using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class PromptService(IRepository<Prompt> promptRepository)
{
    public async Task<Prompt?> GetByIdAsync(int id)
    {
        return await promptRepository.GetById(id);
    }

    public async Task<IEnumerable<Prompt>> GetAllAsync()
    {
        return await promptRepository.GetAll();
    }

    public async Task<IEnumerable<Prompt>> GetAllActiveAsync()
    {
        var prompts = await promptRepository.GetAll();
        return prompts.Where(p => p.IsActive);
    }

    public async Task AddAsync(Prompt prompt)
    {
        await promptRepository.Add(prompt);
    }

    public async Task UpdateAsync(Prompt prompt)
    {
        await promptRepository.Update(prompt);
    }

    public async Task DeleteAsync(Prompt prompt)
    {
        await promptRepository.Delete(prompt);
    }
}
