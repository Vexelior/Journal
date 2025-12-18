namespace Domain.Models;

public class Journal
{
    public int Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
