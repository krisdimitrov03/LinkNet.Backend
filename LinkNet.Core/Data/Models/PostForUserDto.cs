using LinkNet.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Data.Models
{
    public class PostForUserDto
    {
        public string Id { get; set; }


        public UserInPostDto User { get; set; }

        public string[] Images { get; set; }

        public List<Like> Likes { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
