using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Models;
using LinkNet.Infrastructure.Data.Models.Identity;
using LinkNet.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(IApplicationDbRepository _repo,
            SignInManager<ApplicationUser> _signInManager)
        {
            repo = _repo;
            signInManager = _signInManager;
        }

        public async Task<string?> LogUserIn(LoginDto data)
        {
            //if(data.Username == null || data.Password == null)
            //{
            //    return null;
            //}

            var result = await signInManager.PasswordSignInAsync(data.Username, data.Password, false, false);

            if (!result.Succeeded)
            {
                return null;
            }

            return "success";
        }
    }
}
