using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class UserListFilterViewModel
    {
        public List<SignalrUser> Users { get; set; }
        public string UserNameFilter { get; set; }

    }
}
