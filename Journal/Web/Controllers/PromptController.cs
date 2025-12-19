using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "Admin")]
public class PromptController(PromptService service, EntriesService entriesService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var prompts = await service.GetAllAsync();
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
        if (ModelState.IsValid)
        {
            await service.AddAsync(prompt);
            return RedirectToAction(nameof(Index));
        }
        return View(prompt);
    }

    public async Task<IActionResult> Details(int id)
    {
        var prompt = await service.GetByIdAsync(id);
        if (prompt == null)
        {
            return NotFound();
        }
        prompt.JournalEntries = (ICollection<JournalEntry>)await entriesService.GetJournalEntriesByPromptIdAsync(id);
        return View(prompt);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var prompt = await service.GetByIdAsync(id);
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
        if (id != prompt.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            await service.UpdateAsync(prompt);
            return RedirectToAction(nameof(Details), new { id = prompt.Id });
        }
        return View(prompt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var prompt = await service.GetByIdAsync(id);
        if (prompt == null)
        {
            return NotFound();
        }
        await service.DeleteAsync(prompt);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var prompt = await service.GetByIdAsync(id);
        if (prompt == null)
        {
            return NotFound();
        }
        prompt.IsActive = !prompt.IsActive;
        await service.UpdateAsync(prompt);
        return RedirectToAction(nameof(Index));
    }
}
