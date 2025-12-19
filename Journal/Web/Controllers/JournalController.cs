using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Web.Controllers;

[Authorize(Roles = "Admin")]
public class JournalController(JournalService journalService, PromptService promptService, EntriesService entriesService, JournalExportService exportService, DocumentExportService documentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var journals = await journalService.GetAllAsync();
        var fullJournals = new List<Journal>();
        foreach (var journal in journals)
        {
            var fullJournal = await journalService.GetFullJournal(journal.Id);
            if (fullJournal != null)
            {
                fullJournals.Add(fullJournal);
            }
        }
        return View(fullJournals);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Journal journal)
    {
        if (ModelState.IsValid)
        {
            await journalService.AddAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return View(journal);
    }

    public async Task<IActionResult> Details(int id)
    {
        var journal = await journalService.GetFullJournal(id);
        if (journal == null)
        {
            return NotFound();
        }
        return View(journal);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var journal = await journalService.GetFullJournal(id);
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
        if (id != journal.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            await journalService.UpdateAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Details), new { id = journal.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var journal = await journalService.GetByIdAsync(id);
        if (journal == null)
        {
            return NotFound();
        }
        await journalService.DeleteAsync(journal);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportPdf(int id)
    {
        var journal = await journalService.GetFullJournal(id);
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
