using Application.Repository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class EntriesRepository(ApplicationDbContext context) : IEntriesRepository
{

    public async Task<IEnumerable<JournalEntry>> GetAll()
    {
        return await context.JournalEntries.ToListAsync();
    }

    public async Task<JournalEntry?> GetById(int id)
    {
        return await context.JournalEntries.FindAsync(id);
    }

    public async Task Add(JournalEntry entity)
    {
        await context.JournalEntries.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task Update(JournalEntry entity)
    {
        context.JournalEntries.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(JournalEntry entity)
    {
        context.JournalEntries.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetEntriesByJournalIdAsync(int journalId)
    {
        return await context.JournalEntries.Where(entry => entry.JournalId == journalId).ToListAsync();
    }
}
