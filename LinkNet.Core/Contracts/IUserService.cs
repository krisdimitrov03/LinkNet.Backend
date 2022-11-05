using LinkNet.Core.Data.Models;

namespace LinkNet.Core.Contracts
{
    public interface IUserService
    {
        Task<string?> LogUserIn(LoginDto data);
    }
}
