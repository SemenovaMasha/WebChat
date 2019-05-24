using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public SignalrUser User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public SignalrUser Author { get; set; }
        public string AuthorId { get; set; }

        public List<UserLike> UserLikes { get; set; } = new List<UserLike>();
    }
}
