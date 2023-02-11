using Microsoft.AspNetCore.Mvc;

namespace Rasp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Bass()
        {
            return Json(new {test = "1"});
        }
    }
}