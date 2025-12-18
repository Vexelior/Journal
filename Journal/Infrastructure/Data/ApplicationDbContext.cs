using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Journal> Journals { get; set; }
    public DbSet<YearlyJournal> YearlyJournals { get; set; }
    public DbSet<MonthlyJournal> MonthlyJournals { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Prompt> Prompts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<YearlyJournal>()
            .HasOne(y => y.Journal)
            .WithMany(j => j.YearlyJournals)
            .HasForeignKey(y => y.JournalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prompt>()
            .HasOne(p => p.Journal)
            .WithMany(j => j.Prompts)
            .HasForeignKey(p => p.JournalId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<YearlyJournal>()
            .HasIndex(y => new { y.JournalId, y.Year })
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
