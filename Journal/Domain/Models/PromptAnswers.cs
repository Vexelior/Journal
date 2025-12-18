namespace Domain.Models;

public class PromptAnswers
{
    public int Id { get; set; }
    public int PromptId { get; set; }
    public Prompt Prompt { get; set; } = null!;
    public int JournalId { get; set; }
    public Journal Journal { get; set; } = null!;
    public string Answer { get; set; } = string.Empty;
}
