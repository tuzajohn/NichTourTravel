using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class AboutViewModel
    {
        [Required(ErrorMessage = "The about title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The about body is required")]
        public string Body { get; set; }
        public int Index { get; set; }
        public string IsSelected { get; set; }
        public int Id { get; set; }
        public string AddedOn { get; set; }
        public string Message { get; set; }
    }
}
