using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //// GET Home/Index
        //[HttpGet]
        //public IActionResult Index(int n)
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Fetch()
        {
            return View();              // Fetch.cshtml
        }

        public IActionResult CheckName()
        {
            return View();              // CheckName.cshtml
        }

        public IActionResult Employees()
        {
            return View();              // Employees.cshtml
        }

        public IActionResult EmployeeTable()
        {
            return View();              // Employees.cshtml
        }

		public IActionResult Vue()
		{
			return View();              // Vue.cshtml
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
