using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class EntriesService(IEntriesRepository journalEntryRepository)
{
    public async Task<JournalEntry?> GetByIdAsync(int id)
    {
        return await journalEntryRepository.GetById(id);
    }
    public async Task<IEnumerable<JournalEntry>> GetAllAsync()
    {
        return await journalEntryRepository.GetAll();
    }
    public async Task AddAsync(JournalEntry journalEntry)
    {
        await journalEntryRepository.Add(journalEntry);
    }
    public async Task UpdateAsync(JournalEntry journalEntry)
    {
        await journalEntryRepository.Update(journalEntry);
    }
    public async Task DeleteAsync(JournalEntry journalEntry)
    {
        await journalEntryRepository.Delete(journalEntry);
    }

    public async Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId)
    {
        return await journalEntryRepository.GetEntriesByJournalIdAsync(journalId);
    }

    public async Task CreateEntryAsync(int journalId, string content)
    {
        var journalEntry = new JournalEntry
        {
            JournalId = journalId,
            Content = content,
            CreatedAt = DateTime.Now
        };
        await journalEntryRepository.Add(journalEntry);
    }

    public async Task<IEnumerable<JournalEntry>> GetJournalEntriesByPromptIdAsync(int promptId)
    {
        return await journalEntryRepository.GetJournalEntriesByPromptIdAsync(promptId);
    }
}
