using Domain.Models;

namespace Application.Repository;

public interface IPromptRepository : IRepository<Prompt>
{
    Task<IEnumerable<Prompt>> GetPromptsByJournalId(int id);
}
