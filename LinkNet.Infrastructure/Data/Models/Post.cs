using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Infrastructure.Data.Models
{
    public class Post
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [StringLength(350)]
        public string Text { get; set; }

        public IEnumerable<PostImage> PostsImages { get; set; }

        public IEnumerable<Like> Likes { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
