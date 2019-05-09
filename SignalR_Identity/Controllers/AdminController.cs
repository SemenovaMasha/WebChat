using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserService UserService;

        public AdminController(UserService userService)
        {
            UserService = userService;
        }

        public IActionResult Index(UserListFilterViewModel filterViewModel)
        {
            filterViewModel.Users = UserService.GetAll(filterViewModel);
            return View(filterViewModel);
        }
    }
}