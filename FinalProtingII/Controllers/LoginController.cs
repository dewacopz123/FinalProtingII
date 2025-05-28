using FinalProtingII.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalProtingII.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Validate(string USERNAME, string PASSWORD)
        {
            // Dummy login logic
            if (USERNAME == "admin" && PASSWORD == "1234")
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false, message = "Username or password is incorrect." });
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
