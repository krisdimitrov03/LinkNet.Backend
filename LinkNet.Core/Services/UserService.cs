using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Constants;
using LinkNet.Core.Data.Models.Identity;
using LinkNet.Core.Data.Operators;
using LinkNet.Core.Settings;
using LinkNet.Infrastructure.Data.Models;
using LinkNet.Infrastructure.Data.Models.Identity;
using LinkNet.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;
        private readonly JwtSettings jwtSettings;
        private readonly ITokenService tokenService;

        public UserService(IApplicationDbRepository _repo,
            SignInManager<ApplicationUser> _signInManager,
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IUserStore<ApplicationUser> _userStore,
            IOptions<JwtSettings> _jwtSettings,
            ITokenService _tokenService)
        {
            repo = _repo;
            signInManager = _signInManager;
            userManager = _userManager;
            roleManager = _roleManager;
            userStore = _userStore;
            emailStore = GetEmailStore();
            jwtSettings = _jwtSettings.Value;
            tokenService = _tokenService;
        }

        public async Task<string?> LogUserIn(LoginDto data)
        {
            var result = await signInManager.PasswordSignInAsync(data.Username, data.Password, false, false);

            if (!result.Succeeded)
            {
                return null;
            }

            var (user, roles) = await GetUserByUsername(data.Username);

            var tokenData = new JwtUserDto()
            {
                Id = user.Id,
                Username = user.UserName,
                ProfilePicture = user.ImageId == null
                    ? ""
                    : user.Image.Url,
                Roles = string.Join(", ", roles)
            };

            string token = await tokenService.Generate(tokenData, jwtSettings);

            bool isSuccessful = await tokenService.AddToken(user.Id, token);

            if (isSuccessful)
            {
                return token;
            }

            return null;
        }

        public async Task<(string?, Dictionary<string, List<string>>?)> RegisterUser(RegisterDto data)
        {
            var user = Activator.CreateInstance<ApplicationUser>();

            await userStore.SetUserNameAsync(user, data.Username, CancellationToken.None);
            await emailStore.SetEmailAsync(user, data.Email, CancellationToken.None);

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;

            DateTime birthday = DateTime.Now;
            DateTime.TryParseExact(data.Birthday, "dd-MM-yyyy", null, DateTimeStyles.None, out birthday);
            user.Birthday = birthday;

            user.GenderId = data.Gender;

            user.Image = new Image()
            {
                Url = data.ImageUrl
            };

            user.OccupationId = data.Occupation;

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                return (MessageConstants.SuccessStatus, null);
            }

            var errors = new Dictionary<string, List<string>>();

            result.Errors.ToList().ForEach(e =>
            {
                string? errorsTarget = RegexOperator.GetFirstWord(e.Code).ToLower();

                if (errors.ContainsKey(errorsTarget))
                {
                    errors[errorsTarget].Add(e.Description);
                }
                else
                {
                    errors.Add(errorsTarget, new List<string> { e.Description });
                }
            });

            return (null, errors);
        }

        public async Task<bool> LogUserOut(string token)
        {
            try
            {
                await signInManager.SignOutAsync();
                return await tokenService.RemoveToken(token);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return repo.All<ApplicationUser>()
                .FirstOrDefault(u => u.Id == id);
        }

        public async Task<List<UserTableDto>> GetAll()
        {
            var users = await repo.All<ApplicationUser>()
                .Select(u => new UserTableDto
                {
                    Id = u.Id,
                    Image = u.Image.Url,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Gender = u.Gender.Name,
                    Email = u.Email,
                    Birthday = u.Birthday.ToString("dd.MM.yyyy"),
                    Occupation = u.Occupation.Name
                }).ToListAsync();

            if (users == null || users.Count == 0)
                return new List<UserTableDto>();
            else
                return users;
        }

        public async Task<bool> UpdateUser(UserEditModel data)
        {
            var user = await GetUserById(data.Id);

            if (user == null)
                return false;

            try
            {
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;

                await repo.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await GetUserById(id);

            if (user == null)
                return false;

            try
            {
                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ManageRoles(string userId, string[] roles)
        {
            var user = await GetUserById(userId);

            if (user == null)
                return false;

            try
            {
                var userRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, userRoles);

                if (roles.Length > 0)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<(ApplicationUser?, string[])> GetUserByUsername(string username)
        {
            var user = repo.All<ApplicationUser>()
                .Include(u => u.Image)
                .FirstOrDefault(u => u.UserName == username);

            var roles = await userManager.GetRolesAsync(user);

            return (user, roles.ToArray());
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)userStore;
        }

        public async Task CreateRole(string name)
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = name
            });
        }
    }
}
