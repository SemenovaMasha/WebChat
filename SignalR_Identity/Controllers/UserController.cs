using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Filters;
using SignalR_Identity.Models;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly UserService UserService;
        private Task<SignalrUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public UserController(UserManager<SignalrUser> userManager, SignalrContext context, UserService userService)
        {
            _userManager = userManager;
            UserService = userService;

        }

        [HttpGet]
        [TypeFilter(typeof(UserProfileAccessFilter))]
        public IActionResult EditProfile(string id)
        {
            SignalrUser user = _userManager.Users.Include(u => u.CropAvatar).FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        [HttpPost]
        public IActionResult SaveProfile(UserProfileViewModel viewModel)
        {
            if (viewModel.Id == (_userManager.GetUserAsync(HttpContext.User)).Result.Id
                || (_userManager.IsInRoleAsync(_userManager.GetUserAsync(HttpContext.User).Result, "admin").Result))
            {
                SignalrUser user = UserService.SaveUser(viewModel).Result;
                return View("Profile", user);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        public IActionResult Profile(string id)
        {
            SignalrUser user = _userManager.Users.Include(u=>u.CropAvatar).FirstOrDefault(u=>u.Id == id);

            return View(user);
        }
        public IActionResult MyProfile()
        {
            return RedirectToAction("Profile", "User", new {id = GetCurrentUserAsync().Result?.Id});
        }

        public IActionResult List()
        {
            return View(_userManager.Users.Include(u => u.CropAvatar));
        }
        
    }
}