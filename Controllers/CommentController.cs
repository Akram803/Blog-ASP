using Blog.Data.Repositories;
using Blog.Models.Comment;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (vm.MainCommentId == 0)
            { // this is a main comment
                await _commentRepo.AddMainComment(new MainComment
                {
                    PostId = vm.PostId,
                    message = vm.Message,
                    UserId = userId
                });
            }
            else
            {// this is a sub comment
                await _commentRepo.AddSubComment(new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    message = vm.Message,
                    UserId = userId
                });
            }
            await _commentRepo.SaveChanges();

            return RedirectToAction("Post", "home", new { id = vm.PostId });
        }

        public async Task<IActionResult> deleteMainComment(int id)
        {
            var comment = await _commentRepo.GetMainById(id);
            if (comment == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
                return Forbid();

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
                return Forbid();

            var PostId = (await _commentRepo.GetMainById(
                                        comment.MainCommentId
                                        )).PostId;

            _commentRepo.deleteSub(comment);
            await _commentRepo.SaveChanges();


            return RedirectToAction("post", "home", new { id = PostId });
        }

    }
}
