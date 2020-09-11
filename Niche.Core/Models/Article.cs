using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Niche.Core.Models
{
    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        public string ArticleId { get; set; } = Guid.NewGuid().ToString("N");

        public string Title { get; set; }

        public string Body { get; set; }

        public string AuthorId { get; set; }

        public string ImageURl { get; set; }

        public string TagIds { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
