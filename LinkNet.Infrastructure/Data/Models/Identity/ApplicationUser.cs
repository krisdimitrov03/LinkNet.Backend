using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Infrastructure.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public int GenderId { get; set; }

        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }

        [StringLength(150)]
        public string? Biography { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime Birthday { get; set; }

        [Required]
        public int OccupationId { get; set; }
        
        [ForeignKey(nameof(OccupationId))]
        public Occupation Occupation { get; set; }

        [Required]
        public string ImageId { get; set; }

        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
