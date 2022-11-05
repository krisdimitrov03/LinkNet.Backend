using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Constants;
using LinkNet.Core.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkNet.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService service;

        public UserController(IUserService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginDto data)
        {
            var token = await service.LogUserIn(data);

            if(token == null)
            {
                return NotFound(new
                {
                    Message = MessageConstants.InvalidLoginData
                });
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
