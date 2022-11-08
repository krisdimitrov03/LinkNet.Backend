using LinkNet.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Contracts
{
    public interface IPostService
    {
        Task<List<PostForUserDto>> GetByUser(string userId);
    }
}
