using AutoMapper;
using Blog.Data.Repositories;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private PostRepository _postRepo;
        private IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            PostRepository postRepo,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _postRepo = postRepo;
            _mapper = mapper;
        }

        public async Task<IActionResult> index(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();

            user.Posts = await _postRepo.GetByUserId(user.Id);

            return View(user);
        }

    }
}
