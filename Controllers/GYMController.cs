using Microsoft.AspNetCore.Mvc;

namespace prjAjaxDemo.Controllers
{
    public class GYMController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
