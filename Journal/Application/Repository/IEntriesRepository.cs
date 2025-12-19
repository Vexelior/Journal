using Domain.Models;

namespace Application.Repository;

public interface IEntriesRepository : IRepository<JournalEntry>
{
    Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId);
}
