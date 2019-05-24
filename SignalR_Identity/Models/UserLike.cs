using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.Models
{
    public class UserLike
    {
        public Guid Id { get; set; }

        public string SignalrUserId { get; set; }
        public SignalrUser SignalrUser { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
