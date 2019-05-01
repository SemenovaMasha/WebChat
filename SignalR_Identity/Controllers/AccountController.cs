using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR_Identity.Models;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly SignInManager<SignalrUser> _signInManager;

        public AccountController(UserManager<SignalrUser> userManager, SignInManager<SignalrUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                SignalrUser user = new SignalrUser { UserName = model.Name };
                user.BirthDate = model.BirthDate;
                user.CreatingDate = DateTime.Now;

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Rooms");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) 
                        //&& Url.IsLocalUrl(model.ReturnUrl)
                        )
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Rooms");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Rooms");
        }
    }
}
