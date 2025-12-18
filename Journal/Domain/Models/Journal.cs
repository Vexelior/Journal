namespace Domain.Models;

public class Journal
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Prompt> Prompts { get; set; } = new List<Prompt>();
    public ICollection<YearlyJournal> YearlyJournals { get; set; } = new List<YearlyJournal>();

}
