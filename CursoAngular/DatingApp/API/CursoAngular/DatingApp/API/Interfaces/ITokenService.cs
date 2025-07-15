using API.Entities;

namespace API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
    string CreateRefreshToken();
    bool ValidateRefreshToken(string refreshToken, AppUser user);
    void RevokeRefreshToken(string refreshToken, AppUser user);
    void RevokeAllRefreshTokens(AppUser user);

}
