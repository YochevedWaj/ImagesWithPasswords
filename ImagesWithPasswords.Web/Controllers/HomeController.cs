using ImagesWithPasswords.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ImagesWithPasswords.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ImagesWithPasswords.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ImagesWithPasswords;Integrated Security=true;";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload(string password, IFormFile imageFile)
        {
            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var repo = new ImagesRepository(_connectionString);
            var image = new Image { FileName = fileName, Password = password, ViewCount = 1 };
            repo.AddImage(image);
            var imageLink = $"https://localhost:44301/home/ViewImage?id={image.ID}";
            var vm = new UploadViewModel
            {
                ImageLink = imageLink,
                ImagePassword = password
            };
            return View(vm);
        }

       

        public IActionResult ViewImage(int id)
        {
            var repo = new ImagesRepository(_connectionString);
            var image = repo.GetImage(id);
            var ids = HttpContext.Session.Get<List<int>>("ids");
            if (ids == null)
            {
                ids = new List<int>();
            }
            if (ids.Contains(image.ID))
            {
                repo.IncrementImageViewCount(id);
            }
            var vm = new ViewImageViewModel
            {
                ImageID = id,
                CheckForPassword = !ids.Contains(image.ID),
                ImagePath = image.FileName,
                ImageViewCount = image.ViewCount,
                ErrorMessage = (string)TempData["message"]
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult ViewImage(int id, string password)
        {
            var repo = new ImagesRepository(_connectionString);
            var ids = HttpContext.Session.Get<List<int>>("ids");            
            if (!String.IsNullOrEmpty(password) && password == repo.GetPasswrod(id))
            {
                if (ids == null)
                {
                    ids = new List<int>();
                }
                ids.Add(id);
                HttpContext.Session.Set("ids", ids);
            }
            else
            {
                TempData["message"] = "Invalid Password";
            }
            return Redirect($"/home/viewimage?id={id}");
        }
    }
}
