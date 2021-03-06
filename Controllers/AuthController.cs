using AutoMapper;
using Blog.Models;
using Blog.Services.Email;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private EmailService _emailService;
        private IMapper _mapper;

        public AuthController(
            IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            EmailService emailService
            )
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);
            if (result.Succeeded)
            {
                //await _emailService.SendEmail("aminakram379@gmail.com", "welcome", "you loged in now");
                return RedirectToAction("index", "home");
            }
            else
            {

                ModelState.AddModelError("", "Invalid Login Attempt");
                //await _userManager.IsEmailConfirmedAsync();
                return View();
            }
            
            
        }

        public async Task<IActionResult> logOut()
        {
            // use signInManager to set signin cookie as empty
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = _mapper.Map<AppUser>(vm);


            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(vm);
            }


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail" , "Auth", new { token = token, email = user.Email });

            //await _emailService.SendEmail(user.Email,
            //                            "Blogy Email Confirmation", 
            //                            $"Confirmation email link {confirmationLink}");

            return View("SuccessRegistration");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                //return View("Error");
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View();
            else
                return NotFound();
             
        }

        public IActionResult Forbiden()
        {
            return View();
        }
    }
}
