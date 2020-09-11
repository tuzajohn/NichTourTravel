using Niche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Models
{
    public class ArticleViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public string ImageURl { get; set; }

        public string Tags { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        public int ShareCount { get; set; }
        public string Duration { get; set; }
        public string AddedOn { get; set; }
    }
}
