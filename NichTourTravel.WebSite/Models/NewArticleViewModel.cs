using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class NewArticleViewModel
    {
        [Required(ErrorMessage ="Set a title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Set a tag")]
        public string TagIds { get; set; }
        [Required(ErrorMessage = "Set a content")]
        public string Content { get; set; }
        public string Caption { get; set; }

        [Required(ErrorMessage = "Provide an image")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Set a author")]
        public string AuthorId { get; set; }


        public string Message { get; set; }
    }
}
