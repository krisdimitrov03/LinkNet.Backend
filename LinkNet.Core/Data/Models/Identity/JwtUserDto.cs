using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Data.Models.Identity
{
    public class JwtUserDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public string Roles { get; set; }
    }
}
