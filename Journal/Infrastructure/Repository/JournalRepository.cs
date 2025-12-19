using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class JournalRepository(ApplicationDbContext context) : Repository<Journal>(context), IJournalRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Journal?> GetByIdWithEntriesAsync(int id)
    {
        return await _context.Journals.Include(journal => journal.JournalEntries).FirstOrDefaultAsync(journal => journal.Id == id);
    }
}
