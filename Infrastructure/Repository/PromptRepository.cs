using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class PromptRepository(ApplicationDbContext context) : Repository<Prompt>(context), IPromptRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Prompt>> GetPromptsByJournalId(int id)
    {
        return await _context.Prompts.Where(p => p.JournalEntries.Any(je => je.JournalId == id)).ToListAsync();
    }

    public async Task<IEnumerable<Prompt>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Prompts
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Prompt>> GetActiveByUserIdAsync(string userId)
    {
        return await _context.Prompts
            .Where(p => p.UserId == userId && p.IsActive)
            .ToListAsync();
    }
}
