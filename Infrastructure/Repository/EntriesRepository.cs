using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class EntriesRepository(ApplicationDbContext context) : Repository<JournalEntry>(context), IEntriesRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId, string userId)
    {
        return await _context.JournalEntries
            .Where(entry => entry.JournalId == journalId && entry.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetJournalEntriesByPromptIdAsync(int promptId, string userId)
    {
        return await _context.JournalEntries
            .Where(entry => entry.PromptId == promptId && entry.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetAllByUserIdAsync(string userId)
    {
        return await _context.JournalEntries
            .Where(entry => entry.UserId == userId)
            .ToListAsync();
    }
}
