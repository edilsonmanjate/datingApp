using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateRefreshToken()
    {
        throw new NotImplementedException();


    }

    public string CreateToken(AppUser user)
    {
        var tokenKey = configuration["TokenKey"] ?? throw new ArgumentNullException("TokenKey is not configured.");
        if (tokenKey.Length < 64) throw new ArgumentException("Token key must be at least 64 characters long.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
         {
             new(ClaimTypes.Email, user.Email),
             new(ClaimTypes.NameIdentifier, user.Id),
             new(ClaimTypes.Name, user.DisplayName),
         };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public void RevokeAllRefreshTokens(AppUser user)
    {
        throw new NotImplementedException();
    }

    public void RevokeRefreshToken(string refreshToken, AppUser user)
    {
        throw new NotImplementedException();
    }

    public bool ValidateRefreshToken(string refreshToken, AppUser user)
    {
        throw new NotImplementedException();
    }
}
