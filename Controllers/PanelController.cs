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
        private CommentRepository _commentRepo;
        private CategoryRepository _categoryRepo;
        private IFileManager _fileManager;

        public PanelController(
                                PostRepository postRepo, 
                                CategoryRepository catRepo, 
                                CommentRepository commentRepo,
                                IFileManager fileManager
                                )
        {
            _postRepo = postRepo;
            _categoryRepo = catRepo;
            _commentRepo = commentRepo;
            _fileManager = fileManager;
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

        public async Task<IActionResult> deleteMainComment(int id)
        {
            var comment = await _commentRepo.GetMainById(id);
            if (comment == null)
                return NotFound();

            var PostId = comment.PostId;
            _commentRepo.deleteMain(comment);
            await _commentRepo.SaveChanges();

            return RedirectToAction("post", "home", new { id = PostId });
        }


        public async Task<IActionResult> deleteSubComment(int id)
        {
            var comment = await _commentRepo.GetSubById(id);
            if (comment == null)
                return NotFound();

            var PostId = (await _commentRepo.GetMainById(
                                        comment.MainCommentId
                                        )).PostId;

            _commentRepo.deleteSub(comment);
            await _commentRepo.SaveChanges();


            return RedirectToAction("post", "home", new { id = PostId });
        }


    }
}
