using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Web.Controllers;

[Authorize(Roles = "Admin,User")]
public class JournalController(JournalService journalService, DocumentExportService documentService, UserManager<IdentityUser> userManager) : Controller
{
    private string? GetCurrentUserId() => userManager.GetUserId(User);

    public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 9)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journals = await journalService.GetAllByUserIdAsync(userId);
        var fullJournals = new List<Journal>();
        foreach (var journal in journals)
        {
            var fullJournal = await journalService.GetFullJournal(journal.Id, userId);
            if (fullJournal != null)
            {
                fullJournals.Add(fullJournal);
            }
        }
        if (!string.IsNullOrEmpty(search))
        {
            fullJournals = [.. fullJournals.Where(j => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j.Month)
                                           .Contains(search, StringComparison.OrdinalIgnoreCase) || j.Year.ToString().Contains(search))];
        }
        var pagedJournals = fullJournals.Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)fullJournals.Count / pageSize);
        return View(pagedJournals);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Journal journal)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            journal.UserId = userId;
            await journalService.AddAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return View(journal);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journal = await journalService.GetFullJournal(id, userId);
        if (journal == null)
        {
            return NotFound();
        }
        return View(journal);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journal = await journalService.GetFullJournal(id, userId);
        if (journal == null)
        {
            return NotFound();
        }
        return View(journal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Journal journal)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        if (id != journal.Id)
        {
            return NotFound();
        }

        // Verify ownership
        var existingJournal = await journalService.GetByIdAsync(id, userId);
        if (existingJournal == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            journal.UserId = userId;
            await journalService.UpdateAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Details), new { id = journal.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journal = await journalService.GetByIdAsync(id, userId);
        if (journal == null)
        {
            return NotFound();
        }
        await journalService.DeleteAsync(journal);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportPdf(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var journal = await journalService.GetFullJournal(id, userId);
        if (journal == null)
        {
            return NotFound();
        }
        var content = documentService.GeneratePdf(journal);
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);
        var fileName = $"Journal_{monthName}_{journal.Year}.pdf";
        return File(content, "application/pdf", fileName);
    }
}
