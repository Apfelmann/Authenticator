using Authentication.Services;
using AuthenticationBackend.Models;
using JWTAuth.Entities;
using JWTAuth.Models;
using Microsoft.AspNetCore.Authorization;
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

            if (user is null)
            {
                return BadRequest("Username exist");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var response = await authService.LoginAsync(request);
            if (response is null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokenAsync(request);
            if (response is null || response.RefreshToken is null || response.AccessToken is null)
            {
                return Unauthorized("Invalid refresh token");
            }
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }
        // example of an endpoint that requires a specific role
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are an Admin!");
        }
        
    }
}
