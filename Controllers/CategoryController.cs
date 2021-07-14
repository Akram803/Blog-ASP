using Blog.Data.Repositories;
using Blog.Models;
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
    public class CategoryController : Controller
    {
        private CategoryRepository _categoryRepo;

        public CategoryController(CategoryRepository repo)
        {
            _categoryRepo = repo; 
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepo.Getall());
        }

        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                await _categoryRepo.SaveChanges();
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _categoryRepo.Remove(id);
            await _categoryRepo.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
