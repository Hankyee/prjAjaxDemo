using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace prjAjaxDemo.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeworkController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Homework1()
        {

            //string jsonFilePath = Server.MapPath("~/js/travel.json");

            // 讀取 travel.js 檔案中的 JSON 資料
            string jsonFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "js", "travel.js");
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // 從 travel.js 中提取 JSON 資料
            int startIndex = jsonData.IndexOf('{');
            int endIndex = jsonData.LastIndexOf('}');
            jsonData = jsonData.Substring(startIndex, endIndex - startIndex + 1);

            // 將 JSON 資料反序列化為物件
            var spots = JsonConvert.DeserializeObject<dynamic>(jsonData);

            return View(spots);
        }

        public IActionResult Homework2()
        { 
            return View();
        }
        public IActionResult Homework3()
        {
            return View();
        }
        public IActionResult Homework4()
        {
            return View();
        }
    }
}
