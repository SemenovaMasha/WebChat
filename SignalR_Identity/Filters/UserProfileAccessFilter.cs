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

        public UserProfileAccessFilter(UserManager<SignalrUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = (_userManager.GetUserAsync(context.HttpContext.User)).Result.Id== context.RouteData.Values["id"].ToString();
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
