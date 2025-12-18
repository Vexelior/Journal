using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Prompt> Prompts { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>()
                    .HasOne(e => e.Prompt)
                    .WithMany(p => p.JournalEntries)
                    .HasForeignKey(e => e.PromptId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
                    .HasIndex(e => e.EntryDate);
    }
}
