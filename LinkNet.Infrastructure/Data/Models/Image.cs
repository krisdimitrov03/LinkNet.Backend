using System.ComponentModel.DataAnnotations;

namespace LinkNet.Infrastructure.Data.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(30)]
        public string Url { get; set; }
    }
}