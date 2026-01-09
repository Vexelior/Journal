using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Journal> Journals { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Prompt> Prompts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Journal configuration
        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(j => j.Id);

            entity.Property(j => j.Month)
                .IsRequired();

            entity.Property(j => j.Year)
                .IsRequired();

            entity.Property(j => j.CreatedAt)
                .IsRequired();

            entity.Property(j => j.UserId)
                .IsRequired();

            // User relationship - configured in Infrastructure layer
            entity.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(j => j.UserId);
        });

        // Prompt configuration
        modelBuilder.Entity<Prompt>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.UserId)
                .IsRequired();

            // User relationship
            entity.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(p => p.UserId);
        });

        // JournalEntry configuration
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(je => je.Id);

            entity.Property(je => je.Content)
                .IsRequired();

            entity.Property(je => je.EntryDate)
                .IsRequired();

            entity.Property(je => je.CreatedAt)
                .IsRequired();

            // Journal relationship
            entity.HasOne(je => je.Journal)
                .WithMany(j => j.JournalEntries)
                .HasForeignKey(je => je.JournalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prompt relationship
            entity.HasOne(je => je.Prompt)
                .WithMany(p => p.JournalEntries)
                .HasForeignKey(je => je.PromptId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(je => new { je.JournalId, je.PromptId, je.EntryDate });
        });
    }
}
