using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class PromptService(IPromptRepository promptRepository)
{
    public async Task<Prompt?> GetByIdAsync(int id, string userId)
    {
        var prompt = await promptRepository.GetById(id);
        return prompt?.UserId == userId ? prompt : null;
    }

    public async Task<IEnumerable<Prompt>> GetAllByUserIdAsync(string userId)
    {
        return await promptRepository.GetAllByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Prompt>> GetAllActiveAsync(string userId)
    {
        return await promptRepository.GetActiveByUserIdAsync(userId);
    }

    public async Task AddAsync(Prompt prompt)
    {
        await promptRepository.Add(prompt);
    }

    public async Task UpdateAsync(Prompt prompt, string userId)
    {
        // Verify ownership
        var existingPrompt = await promptRepository.GetById(prompt.Id);
        if (existingPrompt?.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot update a prompt you don't own.");
        }
        await promptRepository.Update(prompt);
    }

    public async Task DeleteAsync(Prompt prompt, string userId)
    {
        // Verify ownership
        if (prompt.UserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot delete a prompt you don't own.");
        }
        prompt.IsActive = false;
        await promptRepository.Update(prompt);
    }

    public async Task<IEnumerable<Prompt>> GetPromptsByJournalId(int id)
    {
        return await promptRepository.GetPromptsByJournalId(id);
    }
}
