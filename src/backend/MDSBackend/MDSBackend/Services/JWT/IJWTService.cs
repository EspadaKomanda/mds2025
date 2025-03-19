using System.IdentityModel.Tokens.Jwt;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.JWT;

public interface IJwtService
{ 
    string GenerateAccessToken(ApplicationUser user);
    JwtSecurityToken ValidateAccessToken(string token);
    Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user, string remoteIpAddress);
    Task RevokeRefreshTokenAsync(long userId, string refreshToken, string remoteIpAddress);
}