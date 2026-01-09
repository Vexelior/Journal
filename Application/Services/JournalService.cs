using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class JournalService(IJournalRepository journalRepository)
{
    public async Task<Journal?> GetByIdAsync(int id, string userId)
    {
        var journal = await journalRepository.GetById(id);
        return journal?.UserId == userId ? journal : null;
    }

    public async Task<IEnumerable<Journal>> GetAllByUserIdAsync(string userId)
    {
        return await journalRepository.GetAllByUserIdAsync(userId);
    }

    public async Task AddAsync(Journal journal)
    {
        await journalRepository.Add(journal);
    }

    public async Task UpdateAsync(Journal journal)
    {
        await journalRepository.Update(journal);
    }

    public async Task DeleteAsync(Journal journal)
    {
        await journalRepository.Delete(journal);
    }

    public async Task<Journal?> GetFullJournal(int id, string userId)
    {
        return await journalRepository.GetWithEntriesAndPromptsAsync(id, userId);
    }
}
