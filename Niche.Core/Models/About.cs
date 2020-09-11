using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Niche.Core.Models
{
    public class About
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string AboutId { get; set; } = Guid.NewGuid().ToString("N");
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsSelected { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
