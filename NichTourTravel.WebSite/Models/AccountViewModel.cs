using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Set a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Set a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Set a password")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
