using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_Identity.Controllers
{
    public class UserController : Controller
    {
        public IActionResult EditUserProfile()
        {
            return View();
        }
    }
}