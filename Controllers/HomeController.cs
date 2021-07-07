using Blog.Data;
using Blog.Data.FileMangers;
using Blog.Data.Repositories;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repository, IFileManager fileManager)
        {
            _repo = repository;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _repo.GetAllPosts()
                );
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            return View(
                await _repo.GetPost(id)
                ) ;
        }

        [HttpGet("/images/{name}")]
        public IActionResult Image(string name)
        {
            var type = name.Substring(name.IndexOf(".") + 1);
            return new FileStreamResult(_fileManager.ImageStream(name), $"image/{type}");
        }
    }
}
