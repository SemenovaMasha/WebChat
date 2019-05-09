using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignalR_Identity.Models;

namespace SignalR_Identity.Filters
{
    public class UserProfileAccessFilter : Attribute, IAuthorizationFilter
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserProfileAccessFilter(UserManager<SignalrUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = false;
            if(context.HttpContext.User.Identity.IsAuthenticated)
            hasClaim = (_userManager.GetUserAsync(context.HttpContext.User)).Result?.Id== context.RouteData.Values["id"].ToString()
                           ||(_userManager.IsInRoleAsync(_userManager.GetUserAsync(context.HttpContext.User).Result,"admin").Result);
            if (!hasClaim)
            {
                context.Result = new ViewResult
                {
                    ViewName = "AccessDenied"
                };
            }
        }
    }
}
