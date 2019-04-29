using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class RoomViewModel
    {
        public ChatGroup ChatGroup { get; set; }
        public List<ChatGroup> UserChatGroups { get; set; } = new List<ChatGroup>();
        public string ErrorMessage { get; set; }
    }
}
