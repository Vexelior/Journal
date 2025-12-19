using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class EntriesController(EntriesService service) : Controller
{
    public async Task<IActionResult> EntriesByJournalId(int id)
    {
        var entries = await service.GetEntriesByJournalIdAsync(id);
        return View(entries);
    }
}
