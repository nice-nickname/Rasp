using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
