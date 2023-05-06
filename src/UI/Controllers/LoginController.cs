using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    [Route("/login")]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("AccessDenied");
        }
        return View();
    }

    [Route("/unauthorized")]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return View("Unauthorized");
    }

    [Route("/Account/SignOut")]
    public IActionResult LogOut()
    {
        return RedirectToAction("Login");
    }
}
