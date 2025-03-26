using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.JwtService;
using MDSBackend.Exceptions.UtilServices.JWT;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MDSBackend.Services.JWT;

public class JwtService : IJwtService
{
    #region Fields

    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;
    private readonly UnitOfWork _unitOfWork;
    
    #endregion
   

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger, UnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public string GenerateAccessToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var userRoles = _unitOfWork.UserRoleRepository.Get()
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role)
            .Include(rr => rr.RoleRights)
            .ThenInclude(rr=>rr.Right)
            .ToList();
        
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
            
            foreach (var right in role.RoleRights.Select(rr => rr.Right))
            {
                claims.Add(new Claim("Right", right.Name));
            }
        }

        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["AccessTokenExpirationMinutes"]));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public JwtSecurityToken ValidateAccessToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        
        return validatedToken as JwtSecurityToken;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var dbRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"])),
            Created = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokenRepository.InsertAsync(dbRefreshToken);
        if (!await _unitOfWork.SaveAsync())
        {   
            throw new GenerateRefreshTokenException("Failed to generate refresh token");
        }

        return dbRefreshToken;
    }

    public async Task RevokeRefreshTokenAsync(long userId, string refreshToken, string remoteIpAddress)
    {
        var token = await _unitOfWork.RefreshTokenRepository.Get()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Token == refreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            token.RevokedByIp = remoteIpAddress;
            token.RevokedOn = DateTime.UtcNow;
            
            await _unitOfWork.SaveAsync();
        }
    }
}