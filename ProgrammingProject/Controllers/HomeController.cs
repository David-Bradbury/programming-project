using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Models;
using System.Diagnostics;

namespace ProgrammingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Landing Page
        public IActionResult Index()
        {
            return View();
        }

        // Privacy Policy Page
        public IActionResult Privacy()
        {
            return View();
        }

        // Frequently Asked Questions Page
        public IActionResult FAQ()
        {
            return View();
        }

        // About Us Page
        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}