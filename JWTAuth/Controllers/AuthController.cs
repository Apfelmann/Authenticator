using JWTAuth.Entities;
using JWTAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hashedPaddword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPaddword;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if(user.Username != request.Username)
            {
                return BadRequest("User or Password Wrong");
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user,user.PasswordHash,request.Password)== PasswordVerificationResult.Failed)
            {
                return BadRequest("User or Password Wrong");
            }
            var token="success";
            return Ok(token);
        }
        
    }
}
