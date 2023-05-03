using Microsoft.AspNetCore.Mvc;
using Domain.Api;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;

namespace UI.Controllers;

[Authorize(Roles = "Rasp.Admin")]
public class HomeController : Controller
{
    private readonly IDispatcher _dispatcher;

    public HomeController(IDispatcher dispatcher)
    {
        this._dispatcher = dispatcher;
    }

    public IActionResult Index()
    {
        var faculties = this._dispatcher.Query(new GetFacultiesQuery());

        if (faculties.Count < 1)
        {
            return StatusCode(500, "Возможность работы без факультетов невозможна :(");
        }

        var facultyId = HttpContext.Request.Cookies[GlobalSelectors.FacultyId] ?? string.Empty;

        if (!HttpContext.Request.Cookies.ContainsKey(GlobalSelectors.FacultyId) || faculties.All(s => s.Id.ToString() != HttpContext.Request.Cookies[GlobalSelectors.FacultyId]))
        {
            HttpContext.Response.Cookies.Append(GlobalSelectors.FacultyId, faculties.First().Id.ToString());
            facultyId = faculties.First().Id.ToString();
        }

        this.ViewData["FacultyId"] = facultyId;
        return View();
    }

    [AllowAnonymous]
    [Route("{*url}", Order = 999)]
    public IActionResult PageNotFound()
    {
        // Global 404 NOT FOUND handler
        Response.StatusCode = 404;
        return View();
    }

    [AllowAnonymous]
    [Route("/unauthorized")]
    public IActionResult AccessDenied()
    {
        return View("Unauthorized");
    }

    [AllowAnonymous]
    [Route("/login")]
    public IActionResult Login()
    {
        return Ok("Please, login");
    }
}
