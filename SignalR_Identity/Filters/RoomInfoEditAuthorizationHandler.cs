using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SignalR_Identity.Models;

namespace SignalR_Identity.Filters
{
    public class RoomInfoEditAuthorizationHandler :
        AuthorizationHandler<RoomCreatorRequirement, ChatGroup>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoomCreatorRequirement requirement,
            ChatGroup resource)
        {
            if (context.User.Identity?.Name == resource.Creator.UserName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class RoomCreatorRequirement : IAuthorizationRequirement { }
}
