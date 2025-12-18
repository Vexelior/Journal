using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

//[Authorize(Roles = "Admin")]
public class JournalController(JournalService service) : Controller
{
    public async Task<IActionResult> Index()
    {
        var journals = await service.GetAllAsync();
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
}
