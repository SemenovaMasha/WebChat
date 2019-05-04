using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.Models
{
    public class ChatGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public SignalrUser Creator { get; set; }
    }
}
