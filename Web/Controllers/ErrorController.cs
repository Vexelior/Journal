using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpError(int statusCode)
    {
        var errorViewModel = new ErrorViewModel
        {
            Code = statusCode,
        };

        return View("Error", errorViewModel);
    }

    [Route("Exception/{exception}")]
    public IActionResult Exception(Exception exception)
    {
        var errorViewModel = new ErrorViewModel
        {
            Code = 500,
            Exception = exception
        };

        return View("Error", errorViewModel);
    }
}
