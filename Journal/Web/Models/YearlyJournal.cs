namespace Web.Models;

public class YearlyJournal
{
    public int Id { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<MonthlyJournal> MonthlyJournals { get; set; } = new List<MonthlyJournal>();
}
