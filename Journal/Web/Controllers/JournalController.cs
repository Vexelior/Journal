using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "Admin")]
public class JournalController(JournalService service, EntriesService entriesService, JournalExportService exportService, DocumentExportService documentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var journals = await service.GetAllAsync();
        foreach (var journal in journals)
        {
            var entries = await entriesService.GetEntriesByJournalIdAsync(journal.Id);
            if (entries == null)
            {
                return NotFound();
            }
            journal.JournalEntries = (ICollection<JournalEntry>)entries;
        }
        return View(journals);
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
            await service.AddAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return View(journal);
    }

    public async Task<IActionResult> Details(int id)
    {
        var journal = await service.GetByIdAsync(id);
        if (journal == null)
        {
            return NotFound();
        }
        return View(journal);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var journal = await service.GetByIdAsync(id);
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
            await service.UpdateAsync(journal);
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Details), new { id = journal.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var journal = await service.GetByIdAsync(id);
        if (journal == null) {
            return NotFound();
        }
        await service.DeleteAsync(journal);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportText(int id)
    {
        var journal = await service.GetByIdAsync(id);
        if (journal == null)
        {
            return NotFound();
        }
        var entries = await entriesService.GetEntriesByJournalIdAsync(journal.Id);
        if (entries == null) 
        {
            return NotFound();
        }
        journal.JournalEntries = (ICollection<JournalEntry>)entries;
        var content = exportService.GenerateTextExport(journal);
        var monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);
        var fileName = $"Journal_{monthName}_{journal.Year}.txt";

        return File(content, "text/plain", fileName);
    }

    public async Task<IActionResult> ExportDocx(int id)
    {
        var journal = await service.GetByIdAsync(id);
        if (journal == null)
        {
            return NotFound();
        }
        var entries = await entriesService.GetEntriesByJournalIdAsync(journal.Id);
        if (entries == null)
        {
            return NotFound();
        }
        journal.JournalEntries = (ICollection<JournalEntry>)entries;
        var content = documentService.GenerateDocx(journal);
        var monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);
        var fileName = $"Journal_{monthName}_{journal.Year}.docx";

        return File(content, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
    }

    public async Task<IActionResult> ExportPdf(int id)
    {
        var journal = await service.GetByIdAsync(id);
        if (journal == null)
        {
            return NotFound();
        }
        var entries = await entriesService.GetEntriesByJournalIdAsync(journal.Id);
        if (entries == null)
        {
            return NotFound();
        }
        journal.JournalEntries = (ICollection<JournalEntry>)entries;
        var content = documentService.GeneratePdf(journal);
        var monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);
        var fileName = $"Journal_{monthName}_{journal.Year}.pdf";

        return File(content, "application/pdf", fileName);
    }
}
