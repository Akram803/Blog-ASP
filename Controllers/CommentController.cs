using Blog.Data.Repositories;
using Blog.Models.Comment;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class CommentController : Controller
    {
        private CommentRepository _commentRepo;

        public CommentController(CommentRepository repo)
        {
            _commentRepo = repo;
        } 


        [HttpPost]
        public async Task<IActionResult> Add(CommentViewModel vm) 
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });

            if (vm.MainCommentId == 0)
            { // this is a main comment
                _commentRepo.AddMainComment(new MainComment
                {
                    PostId = vm.PostId,
                    message = vm.Message
                });
            }
            else
            {// this is a sub comment
                _commentRepo.AddSubComment(new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    message = vm.Message
                });
            }
            await _commentRepo.SaveChanges();

            return RedirectToAction("Post", "home", new { id = vm.PostId });
        }
    }
}
