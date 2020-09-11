using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "What is your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "What is your number")]
        public string Number { get; set; }
        [Required(ErrorMessage = "What is your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "What message are you trying to deliver")]
        public string Message { get; set; }
    }
}
