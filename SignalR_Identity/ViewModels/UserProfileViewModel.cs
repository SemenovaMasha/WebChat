using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace SignalR_Identity.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public IFormFile Avatar { get; set; }

    }
}
