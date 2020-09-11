using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Niche.Core.Models
{
    public class Share
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public int? Fb { get; set; }
        public int? Tw { get; set; }
        public int? Wp { get; set; }
    }
}
