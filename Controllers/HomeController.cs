using AutoMapper;
using Blog.Data.FileMangers;
using Blog.Data.Repositories;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private PostRepository _postRepo;
        private CategoryRepository _catRepo;
        private IFileManager _fileManager;
        private IMapper _mapper;

        public HomeController(PostRepository postRepo, 
            CategoryRepository catRepo, 
            IFileManager fileManager,
            IMapper mapper
            )
        { 
            _postRepo = postRepo;
            _catRepo = catRepo;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            [FromQuery] PagingParametersVM pagingParameters,
            int categoryId, 
            string search)
        {

            if (categoryId > 0)
                ViewData["CategoryName"] = (await _catRepo.GetById(categoryId)).Name;
            ViewData["Categories"] = await _catRepo.Getall();

            return View(
                await _postRepo.GetPosts(
                    pagingParameters, categoryId, search
                    )
                );


        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _postRepo.GetByIdFull(id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            ViewData["cats"] = await _catRepo.Getall();

            if (id != null)
            {
                var post = await _postRepo.GetById((int)id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (post.UserId != userId)
                    return NotFound();

                ViewData["postAction"] = "edit";
                return View(
                    _mapper.Map<PostViewModel>(post)
                    );
            }
            ViewData["postAction"] = "create";
            return View(new PostViewModel());
        }

        // create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewData["cats"] = await _catRepo.Getall();
                ViewData["postAction"] = "create";
                return View("CreateOrEdit", vm);
            }

            // Map ViewModel To model
            Post post = _mapper.Map<Post>(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.UserId = userId;

            if (vm.ImageObj != null)
                post.Image = await _fileManager.ImageSave(vm.ImageObj);

            await _postRepo.Add(post);

            await _postRepo.SaveChanges();
            return RedirectToAction("Index");
            
        }
        // create or update
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewData["cats"] = await _catRepo.Getall();
                ViewData["postAction"] = "create";
                return View("CreateOrEdit", vm);
            }

            // Map ViewModel To model
            Post post = _mapper.Map<Post>(vm);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if ((await _postRepo.Check(post.Id)).UserId != userId)
                return NotFound();
            post.UserId = userId;

            if (vm.ImageObj != null)
            {
                if (!string.IsNullOrEmpty(vm.ImageName))
                    _fileManager.ImageDelete(vm.ImageName);

                post.Image = await _fileManager.ImageSave(vm.ImageObj);
            }

            _postRepo.Update(post);

            if (await _postRepo.SaveChanges())
                return RedirectToAction("Index");
            else
            {
                ViewData["cats"] = await _catRepo.Getall();
                ViewData["postAction"] = "create";
                return View("CreateOrEdit", vm);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await _postRepo.GetById(id);

            if (post.UserId != userId)
                return Forbid();

            if (!string.IsNullOrEmpty(post.Image))
                _fileManager.ImageDelete(post.Image);

            _postRepo.Remove(post);
            await _postRepo.SaveChanges();

            return RedirectToAction("index", "home");
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
