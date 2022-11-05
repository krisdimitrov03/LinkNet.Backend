using LinkNet.Infrastructure.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkNet.Infrastructure.Data.Models
{
    public class Comment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(350)]
        public string Text { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; }

        [Required]
        public DateTime DateTimeCreated { get; set; }
    }
}