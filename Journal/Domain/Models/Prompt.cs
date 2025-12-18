namespace Domain.Models;

public class Prompt
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public int? JournalId { get; set; }
    public Journal? Journal { get; set; }
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
