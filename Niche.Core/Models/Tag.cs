using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Niche.Core.Models
{
    public class Tag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string TagId { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
