namespace Domain.Models;

public class Prompt
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
