using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PurpleHackBackend.Exceptions.UtilServices.JWT;
using PurpleHackBackend.Models.DTO;

namespace PurpleHackBackend.Services.JWT;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JWTService> _logger;

    public JWTService(IConfiguration configuration, ILogger<JWTService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public async Task<string> GenerateJwtToken(UserDTO user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new SystemException("JwtSettings:SecretKey not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        if (user.Role == null)
        {
            _logger.LogError("User {Username} has no role", user.Username);
            // TODO: custom exception
            throw new  GenerateJWTTokenException("User has no role");
        }

        claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

        try
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenLifetime"])),
                signingCredentials: creds);

            _logger.LogDebug("Generated JWT token for user {Username}", user.Username);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token for user {Username}", user.Username);
            throw new GenerateJWTTokenException(ex.Message);
        }
    }
}