using Microsoft.AspNetCore.Mvc;

namespace WebApplication_1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
