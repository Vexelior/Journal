namespace Domain.Models;

public class YearlyJournal
{
    public int Id { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int JournalId { get; set; }
    public Journal Journal { get; set; } = null!;
    public ICollection<MonthlyJournal> MonthlyJournals { get; set; } = new List<MonthlyJournal>();
}
