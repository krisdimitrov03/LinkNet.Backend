using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Data.Models.Identity
{
    public class RegisterDto
    {
        public string Username { get; set; }

        public string? ImageUrl { get; set; }

        public string Email { get; set; }

        public int Gender { get; set; }

        public int Occupation { get; set; }

        public string Birthday { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
    }
}
