using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class JournalRepository(ApplicationDbContext context) : Repository<Journal>(context), IJournalRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Journal?> GetWithEntriesAndPromptsAsync(int id, string userId)
    {
        return await _context.Journals
            .Where(journal => journal.Id == id && journal.UserId == userId)
            .Include(journal => journal.JournalEntries)
            .ThenInclude(entry => entry.Prompt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Journal>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Journals
            .Where(journal => journal.UserId == userId)
            .ToListAsync();
    }
}
