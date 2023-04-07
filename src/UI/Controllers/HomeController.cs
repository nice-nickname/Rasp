using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("{*url}", Order = 999)]
    public IActionResult PageNotFound()
    {
        // Global 404 NOT FOUND handler
        Response.StatusCode = 404;
        return View();
    }

    public IActionResult EditFaculty()
    {
        return View("~/Views/Edit/Faculty/Index.cshtml");
    }

    public IActionResult EditDepartment()
    {
        return View("~/Views/Edit/Department/Index.cshtml");
    }
}