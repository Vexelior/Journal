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
}
