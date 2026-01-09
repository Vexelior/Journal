namespace Domain.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime EntryDate { get; set; } = DateTime.Now.Date;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // User ownership
    public string UserId { get; set; } = string.Empty;

    // Foreign keys
    public int JournalId { get; set; }
    public int PromptId { get; set; }

    // Navigation properties
    public Journal Journal { get; set; } = null!;
    public Prompt Prompt { get; set; } = null!;
}
