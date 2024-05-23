using Microsoft.AspNetCore.Mvc;
using prjAjaxDemo.Models;
using System.Diagnostics;

namespace prjAjaxDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _mydbContext;
        public HomeController(ILogger<HomeController> logger, MyDBContext mydbContext)
        {
            _logger = logger;
            _mydbContext = mydbContext;
        }

        public IActionResult Index()
        {
            var products = _mydbContext.Categories;
            return View(products);
        }

        public IActionResult first()
        {
            return View();
        }
        public IActionResult address()
        {
            return View();
        }
        public IActionResult register()
        {
            return View();
        }



        public IActionResult About()
        {
            
            return View();
        }
        public IActionResult spot()
        {
            return View();
        }

        public IActionResult callAPI()
        {
            return View();
        }
        public IActionResult History()
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
    }
}