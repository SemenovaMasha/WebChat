using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SignalR_Identity.Models
{
    public class SignalrUser : IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatingDate { get; set; }
        public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public Image CropAvatar { get; set; }
        public Guid? CropAvatarId { get; set; }
        public List<UserLike> UserLikes { get; set; } = new List<UserLike>();
    }
}
