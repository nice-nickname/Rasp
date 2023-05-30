using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    [Route("/login")]
    public IActionResult Login()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return View();
        }

        if (User.IsInRole("Rasp.Admin"))
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("AccessDenied");
    }

    [Route("/unauthorized")]
    public IActionResult AccessDenied()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Login", "LoginController");
        }

        if (User.IsInRole("Rasp.Admin"))
        {
            return RedirectToAction("Index", "Home");
        }

        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return View("Unauthorized");
    }

    [Route("/Account/SignOut")]
    public IActionResult LogOut()
    {
        return RedirectToAction("Login");
    }
}
