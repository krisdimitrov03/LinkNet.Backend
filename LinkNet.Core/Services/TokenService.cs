using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Models.Identity;
using LinkNet.Core.Settings;
using LinkNet.Infrastructure.Data.Models.Identity;
using LinkNet.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkNet.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IApplicationDbRepository repo;

        public TokenService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> AddToken(string userId, string token)
        {
            try
            {
                await repo.AddAsync(new TokenLog
                {
                    UserId = userId,
                    Token = token
                });

                await repo.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsTokenValid(string token)
        {
            var tokenLog = await repo.All<TokenLog>()
                .FirstOrDefaultAsync(t => t.Token == token);

            if (tokenLog == null) return false;
            else return true;
        }

        public async Task<bool> RemoveToken(string token)
        {
            if (!await IsTokenValid(token))
            {
                return false;
            }

            try
            {
                var tokenLog = await repo
                    .All<TokenLog>()
                    .FirstOrDefaultAsync(t => t.Token == token);

                await repo.DeleteAsync<TokenLog>(tokenLog.Id);

                await repo.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> Generate(JwtUserDto data, JwtSettings jwtSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", data.Id),
                    new Claim("username", data.Username),
                    new Claim("profilePicture", data.ProfilePicture),
                    new Claim("roles", data.Roles)
                }),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
