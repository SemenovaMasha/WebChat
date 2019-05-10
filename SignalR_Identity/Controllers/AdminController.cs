using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserService UserService;
        private readonly UserManager<SignalrUser> _userManager;

        public AdminController(UserService userService, UserManager<SignalrUser> userManager)
        {
            UserService = userService;
            _userManager = userManager;
        }

        public IActionResult Index(UserListIndexViewModel viewModel)
        {
            if (viewModel == null)
                viewModel = new UserListIndexViewModel();

            if (viewModel.PageViewModel == null)
            {
                viewModel.PageViewModel = new PageViewModel()
                {
                    PageSize = 20,
                    PageNumber = 1,
                };
            }

            if (viewModel.FilterViewModel == null)
            {
                viewModel.FilterViewModel = new UserListFilterViewModel();
            }

            TempData["returnUrl"] = Request.GetEncodedPathAndQuery();
            return View(UserService.GetAll(viewModel));
        }

        public IActionResult DeleteUser(string id)
        {
            UserService.RemoveUser(id);
            string returnUrl = Convert.ToString(TempData["returnUrl"]);
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RestoreUser(string id)
        {
            UserService.RestoreUser(id);
            string returnUrl = Convert.ToString(TempData["returnUrl"]);
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
    }
}