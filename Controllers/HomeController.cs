using Blog.Data.FileMangers;
using Blog.Data.Repositories;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private PostRepository _postRepo;
        private CategoryRepository _catRepo;
        private IFileManager _fileManager;

        public HomeController(PostRepository postRepo, CategoryRepository catRepo, IFileManager fileManager)
        {
            _postRepo = postRepo;
            _catRepo = catRepo;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index(
            [FromQuery] PagingParametersVM pagingParameters,
            int categoryId, 
            string search)
        {

            if (categoryId > 0)
                ViewData["CurrentCat"] = await _catRepo.GetById(categoryId);


            var postPagedList = await _postRepo.GetPosts(pagingParameters, categoryId, search) ;

            ViewData["cats"] = await _catRepo.Getall();
            return View(postPagedList);


        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {            
            return View(await _postRepo.GetById(id));
        }


        [HttpGet("/images/{name}")]
        [ResponseCache(CacheProfileName = "weekly")]
        public IActionResult Image(string name)
        {
            var type = name.Substring(name.IndexOf(".") + 1);
            return new FileStreamResult(_fileManager.ImageStream(name), $"image/{type}");
        }


    }
}
