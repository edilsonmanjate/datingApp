using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(AppDbContext dbcontext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await UserExists(registerDto.email))
            {
                return BadRequest("Email already exists");
            }
            var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = registerDto.displayName,
                Email = registerDto.email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key
            };

            dbcontext.Users.Add(user);
            await dbcontext.SaveChangesAsync();

            return user.ToDto(tokenService);

        }

        private async Task<bool> UserExists(string email)
        {
            return await dbcontext.Users.AnyAsync(x => x.Email == email);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dbcontext.Users.SingleOrDefaultAsync(x => x.Email == loginDto.email);
            if (user == null) return Unauthorized("Invalid email");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return user.ToDto(tokenService);

        }
    }
}
