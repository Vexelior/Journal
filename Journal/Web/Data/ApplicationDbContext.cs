using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<YearlyJournal> YearlyJournals { get; set; }
    public DbSet<MonthlyJournal> MonthlyJournals { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Prompt> Prompts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<YearlyJournal>()
                    .HasIndex(y => y.Year)
                    .IsUnique();

        modelBuilder.Entity<MonthlyJournal>()
                    .HasOne(m => m.YearlyJournal)
                    .WithMany(y => y.MonthlyJournals)
                    .HasForeignKey(m => m.YearlyJournalId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlyJournal>()
                    .HasIndex(m => new { m.YearlyJournalId, m.Month })
                    .IsUnique();

        modelBuilder.Entity<JournalEntry>()
                    .HasOne(e => e.MonthlyJournal)
                    .WithMany(m => m.JournalEntries)
                    .HasForeignKey(e => e.MonthlyJournalId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JournalEntry>()
                    .HasOne(e => e.Prompt)
                    .WithMany(p => p.JournalEntries)
                    .HasForeignKey(e => e.PromptId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
                    .HasIndex(e => e.EntryDate);
    }
}
