using Domain.Models;

namespace Application.Repository;

public interface IJournalRepository : IRepository<Journal>
{
    Task<Journal?> GetByIdWithEntriesAsync(int id);
}
