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
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repository, IFileManager fileManager)
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
                );
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var post = await _repo.GetPost((int)id);
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
                Image = vm.ImageName
            };
            // creation 
            if (post.Id == 0)
            {
                if(vm.ImageObj != null)
                    post.Image = await _fileManager.ImageSave(vm.ImageObj); // save image whatever
                _repo.AddPost(post);
            }
            // update
            else
            {
                // if image name is diffrent : delete the olde one then save the new one
                // if image name is same : don't
                if(vm.ImageObj != null )
                {
                    if(!string.IsNullOrEmpty(vm.ImageName))
                        _fileManager.ImageDelete(vm.ImageName);

                    post.Image = await _fileManager.ImageSave(vm.ImageObj);
                }
                _repo.UpdatePost(post);
            }


            if (await _repo.SaveChanges())
                return RedirectToAction("Index");
            else
                return View(post);

        }

        [HttpGet]
        public async Task<IActionResult> delete(int id) 
        {
            var post = await _repo.GetPost(id);

            if( !string.IsNullOrEmpty(post.Image) )
                _fileManager.ImageDelete(post.Image);

            await _repo.RemovePost(post);
            await _repo.SaveChanges();

            return RedirectToAction("index", "panel");
        }

       
    }
}
