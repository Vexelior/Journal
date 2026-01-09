using Domain.Models;

namespace Application.Repository;

public interface IPromptRepository : IRepository<Prompt>
{
    Task<IEnumerable<Prompt>> GetPromptsByJournalId(int id);
    Task<IEnumerable<Prompt>> GetAllByUserIdAsync(string userId);
    Task<IEnumerable<Prompt>> GetActiveByUserIdAsync(string userId);
}
