using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public SignalrUser User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public ChatGroup ChatGroup { get; set; }
    }
}
