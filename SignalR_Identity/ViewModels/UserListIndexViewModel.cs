using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class UserListIndexViewModel
    {
        public IEnumerable<SignalrUser> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public UserListFilterViewModel FilterViewModel { get; set; }
    }
}
