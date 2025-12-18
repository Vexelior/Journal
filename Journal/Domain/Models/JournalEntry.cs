namespace Domain.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime EntryDate { get; set; } = DateTime.Now.Date;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public int MonthlyJournalId { get; set; }
    public int PromptId { get; set; }
    public MonthlyJournal MonthlyJournal { get; set; } = null!;
    public Prompt Prompt { get; set; } = null!;
}
