using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "What could your name be?")]
        public string Name { get; set; }

        [StringLength(50), Required(ErrorMessage = "What could your email be?")]
        public string Email { get; set; }

        [StringLength(50)]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Please leave a message")]
        public string Message { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string DateDuration { get; set; }
    }
}
