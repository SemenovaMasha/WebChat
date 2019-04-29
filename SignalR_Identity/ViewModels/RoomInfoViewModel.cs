using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class RoomInfoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Members { get; set; }
        public List<string> Others { get; set; }
    }
}
