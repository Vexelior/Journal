using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Journal");
        }
        else
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
