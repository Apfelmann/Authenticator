using Authentication.Services;
using JWTAuth.Entities;
using JWTAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if(user is null)
            {
               return BadRequest("Username exist");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
           var token = await authService.LoginAsync(request);
            if (token is null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(token);
        }
    }
}
