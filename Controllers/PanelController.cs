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
    [Authorize]
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
                await _postRepo.GetAll()
                );
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _postRepo.GetById(id);
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["cats"] = await _categoryRepo.Getall();

            if (id != null)
            {
                var post = await _postRepo.GetById((int)id);
                return View(
                    new PostViewModel
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Body = post.Body,
                        ImageName = post.Image
                    });
            }
            return View(new PostViewModel());
        }

        // create or update
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            // Map ViewModel To model
            Post post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                CategoryId = vm.CatrgoryId,
                Image = vm.ImageName
            };

            if (vm.ImageObj != null)
            {
                if (!string.IsNullOrEmpty(vm.ImageName))
                    _fileManager.ImageDelete(vm.ImageName);

                post.Image = await _fileManager.ImageSave(vm.ImageObj);
            }

            // creation 
            if (post.Id == 0)
                await _postRepo.Add(post);
            // update
            else
                _postRepo.Update(post);

            if (await _postRepo.SaveChanges())
                return RedirectToAction("Index");
            else
                return View(post);

        }

        [HttpGet]
        public async Task<IActionResult> delete(int id) 
        {
            var post = await _postRepo.GetById(id);

            if( !string.IsNullOrEmpty(post.Image) )
                _fileManager.ImageDelete(post.Image);

            _postRepo.Remove(post);
            await _postRepo.SaveChanges();

            return RedirectToAction("index", "panel");
        }

    }
}
