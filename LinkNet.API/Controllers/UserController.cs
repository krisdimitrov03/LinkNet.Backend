using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Constants;
using LinkNet.Core.Data.Models;
using LinkNet.Core.Data.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkNet.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService service;
        private readonly ITokenService tokenService;

        public UserController(IUserService _service,
            ITokenService _tokenService)
        {
            service = _service;
            tokenService = _tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto data)
        {
            var token = await service.LogUserIn(data);

            if (token == null)
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

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto data)
        {
            var (result, errors) = await service.RegisterUser(data);

            if (result == null)
            {
                return BadRequest(new { Errors = errors });
            }

            return Ok(new { Status = result });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout([FromHeader] string? Authorization)
        {
            var token = Authorization.Replace("Bearer ", string.Empty);

            if (!await tokenService.IsTokenValid(token))
                return Unauthorized(new
                {
                    Message = "Invalid token!"
                });

            var result = await service.LogUserOut(token);

            if (!result)
                return BadRequest(new
                {
                    Message = "Logging out not successful."
                });

            return Ok(new { Status = "success" });
        }
    }
}
