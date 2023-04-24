using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class ScheduleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
