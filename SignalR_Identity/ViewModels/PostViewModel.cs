using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Liked { get; set; }
        public bool UserAuth { get; set; }
        public string UserId { get; set; }
        public string CurrentUsername { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarPath { get; set; }
        public bool IsOwner { get; set; }
    }
}
