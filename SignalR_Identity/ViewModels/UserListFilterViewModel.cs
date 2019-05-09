using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class UserListFilterViewModel
    {
        public List<SignalrUser> Users { get; set; }
        public string UserNameFilter { get; set; }
        public bool OnlyDeleted { get; set; }
        public bool OnlyNotDeleted { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDateRangeStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDateRangeEnd { get; set; }
    }
}
