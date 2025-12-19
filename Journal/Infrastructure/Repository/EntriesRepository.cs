using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class EntriesRepository(ApplicationDbContext context) : Repository<JournalEntry>(context), IEntriesRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId)
    {
        return await _context.JournalEntries.Where(entry => entry.JournalId == journalId).ToListAsync();
    }
}
