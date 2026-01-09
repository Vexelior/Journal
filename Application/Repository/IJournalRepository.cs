using Application.Repository;
using Domain.Models;

namespace Application.Repository;

public interface IJournalRepository : IRepository<Journal>
{
    Task<Journal?> GetWithEntriesAndPromptsAsync(int id, string userId);
    Task<IEnumerable<Journal>> GetAllByUserIdAsync(string userId);
}
