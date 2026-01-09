using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class EntriesService(IEntriesRepository journalEntryRepository, IJournalRepository journalRepository)
{
    public async Task<JournalEntry?> GetByIdAsync(int id, string userId)
    {
        var entry = await journalEntryRepository.GetById(id);
        if (entry == null) return null;
        
        // Verify the entry belongs to a journal owned by the user
        var journal = await journalRepository.GetById(entry.JournalId);
        return journal?.UserId == userId ? entry : null;
    }
    
    public async Task<IEnumerable<JournalEntry>> GetAllAsync()
    {
        return await journalEntryRepository.GetAll();
    }
    
    public async Task AddAsync(JournalEntry journalEntry, string userId)
    {
        // Verify the journal belongs to the user
        var journal = await journalRepository.GetById(journalEntry.JournalId);
        if (journal?.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot add entry to a journal you don't own.");
        }
        
        await journalEntryRepository.Add(journalEntry);
    }
    
    public async Task UpdateAsync(JournalEntry journalEntry, string userId)
    {
        // Verify the entry belongs to a journal owned by the user
        var journal = await journalRepository.GetById(journalEntry.JournalId);
        if (journal?.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot update an entry you don't own.");
        }
        
        await journalEntryRepository.Update(journalEntry);
    }
    
    public async Task DeleteAsync(JournalEntry journalEntry, string userId)
    {
        // Verify the entry belongs to a journal owned by the user
        var journal = await journalRepository.GetById(journalEntry.JournalId);
        if (journal?.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot delete an entry you don't own.");
        }
        
        await journalEntryRepository.Delete(journalEntry);
    }

    public async Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId, string userId)
    {
        // Verify the journal belongs to the user
        var journal = await journalRepository.GetById(journalId);
        if (journal?.UserId != userId)
        {
            return Enumerable.Empty<JournalEntry>();
        }
        
        return await journalEntryRepository.GetEntriesByJournalIdAsync(journalId);
    }

    public async Task CreateEntryAsync(int journalId, string content, string userId)
    {
        // Verify the journal belongs to the user
        var journal = await journalRepository.GetById(journalId);
        if (journal?.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot add entry to a journal you don't own.");
        }
        
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
