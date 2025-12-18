namespace Web.Models;

public class Prompt
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
