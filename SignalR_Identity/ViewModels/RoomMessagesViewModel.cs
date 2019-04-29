using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Identity.Models;

namespace SignalR_Identity.ViewModels
{
    public class RoomMessagesViewModel
    {
        public ChatGroup ChatGroup { get; set; }
        public string ErrorMessage { get; set; }
    }
}
