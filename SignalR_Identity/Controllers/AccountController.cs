using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Extensions;
using SignalR_Identity.Models;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly SignInManager<SignalrUser> _signInManager;
        private Task<SignalrUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

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
                    return RedirectToAction("MyProfile", "User");
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
            object errData = TempData["ModelErrorAccountIsDeleted"];
            if (errData != null)
            {
                ModelState.AddModelError("", errData.ToString());
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl , AuthenticationSchemes = HttpContext.GetExternalProvidersAsync().Result });
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithProvider([FromForm] string provider, [FromForm] string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
            
        }

        public IActionResult CheckIfDeleted()
        {
            if (GetCurrentUserAsync().Result.IsDeleted)
            {
                _signInManager.SignOutAsync().Wait();
                
                TempData["ModelErrorAccountIsDeleted"] = "Your account was deleted";
                return RedirectToAction("Login");
                //return View("Login",new LoginViewModel(){ AuthenticationSchemes = HttpContext.GetExternalProvidersAsync().Result });
            }
            return RedirectToAction("MyProfile", "User");
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                //ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            

            if (result.Succeeded)
            {

                return RedirectToAction("CheckIfDeleted");
            }
            if (result.IsLockedOut)
            {
                return BadRequest();
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                
                return View("ExternalLogin", new ExternalLoginViewModel {  });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new SignalrUser { UserName = model.UserName, Email = model.Email, BirthDate = model.BirthDate, CreatingDate = DateTime.Now,IsDeleted = false};
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("MyProfile", "User");
                    }
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View("ExternalLogin", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(model.Name).Result;
                if (user.IsDeleted)
                {
                    ModelState.AddModelError("", "Your account was deleted");
                }
                else
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
                            return RedirectToAction("MyProfile", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect login or password");
                    }

                }
            }
            model.AuthenticationSchemes = HttpContext.GetExternalProvidersAsync().Result ;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Rooms");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
