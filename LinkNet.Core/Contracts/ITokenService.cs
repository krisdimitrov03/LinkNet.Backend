using LinkNet.Core.Data.Models.Identity;
using LinkNet.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Contracts
{
    public interface ITokenService
    {
        Task<bool> IsTokenValid(string token);
        Task<bool> AddToken(string userId, string token);
        Task<bool> RemoveToken(string token);
        Task<string> Generate(JwtUserDto data, JwtSettings jwtSettings);
    }
}
