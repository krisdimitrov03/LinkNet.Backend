using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Data.Models.Identity
{
    public class UserTableDto
    {
        public string Id { get; set; }

        public string Image { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Birthday { get; set; }

        public string Occupation { get; set; }
    }
}
