using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using prjAjaxDemo.Models;
using System.Text;
using System.Text.Unicode;
using prjAjaxDemo.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace prjAjaxDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _mydbContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ApiController(MyDBContext mydbContext, IWebHostEnvironment hostEnvironment)
        {
            _mydbContext = mydbContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(10000);
            return Content("<h2>嗨嗨~ Word!!</h2>","text/html",Encoding.UTF8);
        }



        //讀出不會重複的城市名
        public IActionResult Cities()
        {
            var cities = _mydbContext.Addresses.Select(x => x.City).Distinct();
            return Json(cities);
        }
        //讀出不會重複的鄉鎮區
        public IActionResult Districts(string city)
        {
            var districts = _mydbContext.Addresses.Where(a=>a.City==city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }
        //根據鄉鎮區讀出路名
        public IActionResult Roads(string districts)
        {
            var roads = _mydbContext.Addresses.Where(b => b.SiteId == districts).Select(b => b.Road).Distinct();
            return Json(roads);
        }


        public IActionResult Avatar(int id=1) 
        {
            Member? member = _mydbContext.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                if(img != null)
                {
                    return File(img, "image/jpeg");
                }
            }
            return NotFound();
        }


        //public IActionResult Register(string userName, string email, int age = 20)
        public IActionResult Register(Member member,IFormFile avatar)
        {
            if (string.IsNullOrEmpty(member.Name)) member.Name = "guest";
            if (avatar == null || avatar.Length == 0)
            {
                return Content("請上傳文件", "text/plain", System.Text.Encoding.UTF8);
            }
            //return Content($"{member.Name}你好，你{member.Age}歲了!電子郵件是 {member.Email}", "text/html", Encoding.UTF8);

            //取得上傳檔案的資訊
            //string info = $"{avatar.FileName} - {avatar.Length} - {avatar.ContentType}";
            //string info = _hostEnvironment.ContentRootPath;

            //檔案上傳
            //檔案上傳寫進資料夾
            //todo1 判斷檔案是否存在
            //todo2 限制上傳檔案的大小跟類型 

            //實際路徑
            //string uploadPath = @"C:\Users\User\Documents\workspace\MSIT158Site\wwwroot\uploads\a.png";
            //WebRootPath：C: \Users\User\Documents\workspace\MSIT158Site\wwwroot
            //ContentRootPath：C:\Users\User\Documents\workspace\MSIT158Site
            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
            string info = uploadPath;
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                avatar.CopyTo(fileStream);
            }

            //檔案上傳轉成二進位
            byte[]? imgByte = null;
            using (var memorystream = new MemoryStream())
            {
                avatar.CopyTo(memorystream);
                imgByte = memorystream.ToArray();
            }
            member.FileName = avatar.FileName;
            member.FileData = imgByte;
            //寫進資料庫
            _mydbContext.Members.Add(member);
            _mydbContext.SaveChanges();

            return Content(info, "text/plain", System.Text.Encoding.UTF8);


            // return Content($"Hello {member.Name}，{member.Age} 歲了，電子郵件是 {member.Email}", "text/html", System.Text.Encoding.UTF8);

        }


        [HttpPost]
        public IActionResult spot([FromBody] searchDTO searchDTO)
        {
            //根據分類編號搜尋景點資料
            var spots = searchDTO.categoryId == 0 ? _mydbContext.SpotImagesSpots : _mydbContext.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryId);

            //根據關鍵字搜尋景點資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) || s.SpotDescription.Contains(searchDTO.keyword));
            }

            //總共有多少筆資料
            int totalCount = spots.Count();
            //每頁要顯示幾筆資料
            int pageSize = (int)searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = (int)searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

            //包裝要傳給client端的資料
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalCount = totalCount;
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();

            //return Json(search);
            return Json(spotsPaging);
        }

        [HttpPost]
        public IActionResult CheckAccount([FromBody] string name)
        {
            var member = _mydbContext.Members.FirstOrDefault(m => m.Name == name);
            if (member != null)
            {
                return Json("帳號已存在");
            }
            else
            {
                return Json("帳號可使用");
            }
            
        }
    }
}
