using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Data;
using JWTAuth.Entities;
using JWTAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Services
{
    public class AuthService(AppDbContext dbContext, IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await dbContext.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower()))
            {
                return null;
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username
            };

            var hashedPaddword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHash = hashedPaddword;

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());

            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }
    }
}
