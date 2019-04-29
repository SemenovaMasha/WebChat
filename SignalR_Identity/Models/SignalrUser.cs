using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SignalR_Identity.Models
{
    public class SignalrUser : IdentityUser
    {
        public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
