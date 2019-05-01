using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

    }
}
