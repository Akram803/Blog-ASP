using Blog.Data.FileMangers;
using Blog.Data.Repositories;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize(Roles = "admin")]
    public class PanelController : Controller
    {
        private PostRepository _postRepo;
        private CategoryRepository _categoryRepo;
        private IFileManager _fileManager;

        public PanelController(
                                PostRepository postRepo, 
                                CategoryRepository catRepo, 
                                IFileManager fileManager
                                )
        {
            _postRepo = postRepo;
            _categoryRepo = catRepo;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _postRepo.GetPosts(new PagingParametersVM(), 0, "")
                );
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            return RedirectToAction("Post", "Home", new { id = id });
        }


        [HttpGet]
        public async Task<IActionResult> delete(int id) 
        {
            var post = await _postRepo.GetById(id);

            if( !string.IsNullOrEmpty(post.Image) )
                _fileManager.ImageDelete(post.Image);

            _postRepo.Remove(post);
            await _postRepo.SaveChanges();

            return RedirectToAction("index", "home");
        }

    }
}
