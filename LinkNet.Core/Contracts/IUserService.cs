using LinkNet.Core.Data.Models.Identity;
using LinkNet.Infrastructure.Data.Models.Identity;

namespace LinkNet.Core.Contracts
{
    public interface IUserService
    {
        Task<string?> LogUserIn(LoginDto data);
        Task<(string, Dictionary<string, List<string>>)> RegisterUser(RegisterDto data);
        Task<bool> LogUserOut(string token);
        Task<ApplicationUser?> GetUserById(string id);
        Task<List<UserTableDto>> GetAll();
        Task<bool> UpdateUser(UserEditModel data);
        Task<bool> DeleteUser(string id);
        Task<bool> ManageRoles(string userId, string[] roles);
        Task CreateRole(string name);
    }
}
