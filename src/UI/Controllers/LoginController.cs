using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    [Route("/login")]
    public IActionResult Index()
    {
        return View();
    }
}
