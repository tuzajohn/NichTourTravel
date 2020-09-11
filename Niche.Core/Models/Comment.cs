using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Niche.Core.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string CommentId { get; set; } = Guid.NewGuid().ToString("N");

        public string ArticleId { get; set; }

        public string ImageUrl { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Subject { get; set; }

        public bool IsApproved { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
