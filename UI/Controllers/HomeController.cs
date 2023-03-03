using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
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
    }
}