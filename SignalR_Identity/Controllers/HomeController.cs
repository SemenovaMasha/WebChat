using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR_Identity.Models;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly MessageService MessageService;
        private readonly RoomService RoomService;

        public HomeController(UserManager<SignalrUser> userManager, MessageService messageService, RoomService roomService)
        {
            _userManager = userManager;
            MessageService = messageService;
            RoomService = roomService;
        }

        public IActionResult Index()
        {
            //RoomService.CreateChatGroup("testGroup", new List<string>());
            //RoomService.CreateChatGroup("testGroup2", _userManager.Users.Select(u => u.UserName).ToList());
            //RoomService.CreateChatGroup("testGroup3", _userManager.Users.Select(u => u.UserName).ToList());
            //RoomService.CreateChatGroup("testGroup4", _userManager.Users.Select(u => u.UserName).ToList());
            //RoomService.CreateChatGroup("testGroup5", _userManager.Users.Select(u => u.UserName).Take(2).ToList());

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
