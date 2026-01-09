using Domain.Models;

namespace Application.Repository;

public interface IEntriesRepository : IRepository<JournalEntry>
{
    Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId, string userId);
    Task<IEnumerable<JournalEntry>> GetJournalEntriesByPromptIdAsync(int promptId, string userId);
    Task<IEnumerable<JournalEntry>> GetAllByUserIdAsync(string userId);
}
