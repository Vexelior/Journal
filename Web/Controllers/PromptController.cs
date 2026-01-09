using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "Admin,User")]
public class PromptController(PromptService service, EntriesService entriesService, UserManager<IdentityUser> userManager) : Controller
{
    private string? GetCurrentUserId() => userManager.GetUserId(User);

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var prompts = await service.GetAllByUserIdAsync(userId);
        return View(prompts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Prompt prompt)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            prompt.UserId = userId;
            await service.AddAsync(prompt);
            return RedirectToAction(nameof(Index));
        }
        return View(prompt);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var prompt = await service.GetByIdAsync(id, userId);
        if (prompt == null)
        {
            return NotFound();
        }
        prompt.JournalEntries = (ICollection<JournalEntry>)await entriesService.GetJournalEntriesByPromptIdAsync(id, userId);
        return View(prompt);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var prompt = await service.GetByIdAsync(id, userId);
        if (prompt == null)
        {
            return NotFound();
        }
        return View(prompt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Prompt prompt)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (id != prompt.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                prompt.UserId = userId;
                await service.UpdateAsync(prompt, userId);
                return RedirectToAction(nameof(Details), new { id = prompt.Id });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        return View(prompt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var prompt = await service.GetByIdAsync(id, userId);
        if (prompt == null)
        {
            return NotFound();
        }

        try
        {
            await service.DeleteAsync(prompt, userId);
            return RedirectToAction(nameof(Index));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var prompt = await service.GetByIdAsync(id, userId);
        if (prompt == null)
        {
            return NotFound();
        }

        try
        {
            prompt.IsActive = !prompt.IsActive;
            await service.UpdateAsync(prompt, userId);
            return RedirectToAction(nameof(Index));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
