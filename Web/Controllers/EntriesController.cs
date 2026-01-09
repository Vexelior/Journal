using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

[Authorize(Roles = "Admin,User")]
public class EntriesController(EntriesService service, PromptService promptService, JournalService journalService, UserManager<IdentityUser> userManager) : Controller
{
    private string? GetCurrentUserId() => userManager.GetUserId(User);

    public async Task<IActionResult> EntriesByJournalId(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journal = await journalService.GetByIdAsync(id, userId);
        if (journal == null) return NotFound();

        var entries = await service.GetEntriesByJournalIdAsync(id, userId);
        ViewBag.Journal = journal;

        foreach (var entry in entries)
        {
            var prompt = await promptService.GetByIdAsync(entry.PromptId);
            ViewBag.Prompts ??= new Dictionary<int, Prompt>();
            if (prompt != null && !ViewBag.Prompts.ContainsKey(prompt.Id))
            {
                ViewBag.Prompts[prompt.Id] = prompt;
            }
        }

        return View(entries);
    }

    public async Task<IActionResult> Create(int journalId)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (journalId <= 0)
        {
            return BadRequest("Invalid journal ID.");
        }

        // Verify journal ownership
        var journal = await journalService.GetByIdAsync(journalId, userId);
        if (journal == null)
        {
            return NotFound();
        }

        ViewBag.JournalId = journalId;

        var prompts = await promptService.GetAllActiveAsync();
        ViewBag.Prompts = new SelectList(prompts, "Id", "Text");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int journalId, DateTime entryDate, int promptId, string content)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            var entry = new JournalEntry
            {
                JournalId = journalId,
                EntryDate = entryDate,
                PromptId = promptId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            try
            {
                await service.AddAsync(entry, userId);
                return RedirectToAction("EntriesByJournalId", new { id = journalId });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        ViewBag.JournalId = journalId;
        var prompts = await promptService.GetAllActiveAsync();
        ViewBag.Prompts = new SelectList(prompts, "Id", "Text", promptId);
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var entry = await service.GetByIdAsync(id, userId);
        if (entry == null)
        {
            return NotFound();
        }
        var prompt = await promptService.GetByIdAsync(entry.PromptId);
        ViewBag.Prompt = prompt;
        return View(entry);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var entry = await service.GetByIdAsync(id, userId);
        if (entry == null)
        {
            return NotFound();
        }
        var prompt = await promptService.GetByIdAsync(entry.PromptId);
        ViewBag.Prompt = prompt;
        return View(entry);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DateTime entryDate, string content)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var entry = await service.GetByIdAsync(id, userId);
        if (entry == null)
        {
            return NotFound();
        }
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError("content", "Content is required.");
            return View(entry);
        }
        entry.EntryDate = entryDate;
        entry.Content = content;
        
        try
        {
            await service.UpdateAsync(entry, userId);
            return RedirectToAction(nameof(Details), new { id = entry.Id });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var entry = await service.GetByIdAsync(id, userId);
        if (entry == null)
        {
            return NotFound();
        }
        
        try
        {
            await service.DeleteAsync(entry, userId);
            return RedirectToAction("EntriesByJournalId", new { id = entry.JournalId });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
