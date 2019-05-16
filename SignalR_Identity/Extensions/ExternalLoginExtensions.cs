using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalR_Identity.Extensions
{
    public static class ExternalLoginExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return null;

            string ret = "";

            try
            {
                ret = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (System.Exception)
            {
            }
            return ret;
        }
    }
}
