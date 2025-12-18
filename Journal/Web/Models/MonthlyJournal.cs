namespace Web.Models;

public class MonthlyJournal
{
    public int Id { get; set; }
    public int Month { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int YearlyJournalId { get; set; }
    public YearlyJournal YearlyJournal { get; set; } = null!;
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
