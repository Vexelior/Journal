using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

[Authorize(Roles = "Admin")]
public class EntriesController(EntriesService service, PromptService promptService, JournalService journalService) : Controller
{
    public async Task<IActionResult> EntriesByJournalId(int id)
    {
        var entries = await service.GetEntriesByJournalIdAsync(id);
        var journal = await journalService.GetByIdAsync(id);
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
        if (journalId <= 0)
        {
            return BadRequest("Invalid journal ID.");
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

            await service.AddAsync(entry);
            return RedirectToAction("EntriesByJournalId", new { id = journalId });
        }

        ViewBag.JournalId = journalId;
        var prompts = await promptService.GetAllActiveAsync();
        ViewBag.Prompts = new SelectList(prompts, "Id", "Text", promptId);
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        var entry = await service.GetByIdAsync(id);
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
        var entry = await service.GetByIdAsync(id);
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
        var entry = await service.GetByIdAsync(id);
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
        await service.UpdateAsync(entry);
        return RedirectToAction("EntriesByJournalId", new { id = entry.JournalId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await service.GetByIdAsync(id);
        if (entry == null)
        {
            return NotFound();
        }
        await service.DeleteAsync(entry);
        return RedirectToAction("EntriesByJournalId", new { id = entry.JournalId });
    }
}
