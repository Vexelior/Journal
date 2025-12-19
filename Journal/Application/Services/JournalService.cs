using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class JournalService(IJournalRepository journalRepository)
{
    public async Task<Journal?> GetByIdAsync(int id)
    {
        return await journalRepository.GetById(id);
    }

    public async Task<IEnumerable<Journal>> GetAllAsync()
    {
        return await journalRepository.GetAll();
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

    public async Task<Journal?> GetFullJournal(int id)
    {
        return await journalRepository.GetWithEntriesAndPromptsAsync(id);
    }
}
