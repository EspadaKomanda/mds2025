using MDSBackend.Models.BasicResponses;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.JWT;
using MDSBackend.Services.TFA;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MDSBackend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    #region Services
    
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ITwoFactorService _twoFactorAuthService;
    
    #endregion
    
    #region Constructor

    public AuthController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService, ITwoFactorService twoFactorAuthService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _twoFactorAuthService = twoFactorAuthService;
    }
    
    #endregion
    
    #region Actions
    
    /// <summary>
    /// Handles user registration.
    /// </summary>
    /// <param name="model">The registration model.</param>
    /// <returns>A response indicating the result of the registration.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO model)
    {
        try
        {
            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                TwoFactorEnabled = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {Errors}", result.Errors);
                return BadRequest(new BasicResponse
                {
                    Code = 400,
                    Message = "User registration failed"
                });
            }

            _logger.LogInformation("User registered successfully: {Username}", model.Username);
            return Ok(new BasicResponse
            {
                Code = 200,
                Message = "User created successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration: {Message}", ex.Message);
            return StatusCode(500, new BasicResponse
            {
                Code = 500,
                Message = "An error occurred during user registration"
            });
        }
    }
    
    
    /// <summary>
    /// Handles user login.
    /// </summary>
    /// <param name="model">The login model.</param>
    /// <returns>A response indicating the result of the login.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user == null)
            {
                _logger.LogError("Invalid username or password");
                return BadRequest(new BasicResponse
                {
                    Code = 400,
                    Message = "Invalid username or password"
                });
            }
            
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            
            if (!passwordCheck)
            {
                _logger.LogError("Invalid username or password");
                return BadRequest(new BasicResponse
                {
                    Code = 400,
                    Message = "Invalid username or password"
                });
            }
            
            if (user.TwoFactorEnabled)
            {
                var code = _twoFactorAuthService.GenerateTwoFactorCode(user);
                await _twoFactorAuthService.SendTwoFactorNotificationAsync(user, code);
                
                _logger.LogInformation("Two-factor authentication required for user {Username}", model.Username);
                return Ok(new LoginResultDTO()
                {
                    RequiresTwoFactorAuth = true,
                    Success = true
                });
            }
            
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(user);
            
            _logger.LogInformation("User logged in successfully: {Username}", model.Username);
            return Ok(new LoginResultDTO()
            {
                RequiresTwoFactorAuth = false,
                Success = true,
                Token = new RefreshTokenDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login: {Message}", ex.Message);
            return StatusCode(500, new BasicResponse
            {
                Code = 500,
                Message = "An error occurred during user login"
            });
        }
    }
    #endregion
}