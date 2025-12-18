namespace Web.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime EntryDate { get; set; } = DateTime.UtcNow.Date;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int PromptId { get; set; }
    public Prompt Prompt { get; set; } = null!;
}
