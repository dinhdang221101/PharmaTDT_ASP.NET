using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebMVC1.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC1.Controllers
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

        public IActionResult ListOrder()
        {
            return View();
        }

        public IActionResult Details()
        {
             return View(); 
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Store()
        {
            return View();
        }

        public IActionResult QLSP()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult QLCTSP()
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