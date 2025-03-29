using MDSBackend.Models.BasicResponses;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.JWT;
using MDSBackend.Services.NotificationService;
using MDSBackend.Services.TFA;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;
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
    private readonly INotificationService _notificationService;
    private readonly MailNotificationsFactory _mailNotificationsFactory;
    private readonly PushNotificationsFactory _pushNotificationsFactory;
    
    
    #endregion
    
    #region Constructor

    public AuthController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService, ITwoFactorService twoFactorAuthService, INotificationService notificationService, MailNotificationsFactory mailNotificationsFactory, PushNotificationsFactory pushNotificationsFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _twoFactorAuthService = twoFactorAuthService;
        _notificationService = notificationService;
        _mailNotificationsFactory = mailNotificationsFactory;
        _pushNotificationsFactory = pushNotificationsFactory;
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
                return Ok(new LoginResultResponse()
                {
                    RequiresTwoFactorAuth = true,
                    Success = true
                });
            }
            
            var accessToken = _jwtService.GenerateAccessToken(user);
            
            _logger.LogInformation("User logged in successfully: {Username}", model.Username);
            await _signInManager.SignInAsync(user, false,"JWT");
            return Ok(new LoginResultResponse()
            {
                RequiresTwoFactorAuth = false,
                Success = true,
                Token = accessToken
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

    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactorCode([FromBody] TwoFactorDTO model)
    {
        try
        {
            if (model.Username != null)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !_twoFactorAuthService.ValidateTwoFactorCode(user, model.Code))
                {
                    return BadRequest(new BasicResponse()
                    {
                        Code = 400,
                        Message = "Invalid code"
                    });
                }
                
                var token = _jwtService.GenerateAccessToken(user);
                var refreshToken =  await _jwtService.GenerateRefreshTokenAsync(user);
                await _signInManager.SignInAsync(user, false,"2FA");
                return Ok( new LoginResultResponse()
                {
                    RequiresTwoFactorAuth = false,
                    Success = true,
                    Token = new RefreshTokenDTO()
                    {
                        AccessToken = token,
                        RefreshToken = refreshToken.Token
                    }
                });
            }
            _logger.LogError("Username can't be empty");
            return BadRequest(new BasicResponse()
            {
                Code = 400,
                Message = "Username can't be empty"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user verification: {Message}", ex.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "An error occurred during user verification"
            });
        }
    }


    private async Task SendNotificationAsync(ApplicationUser user, 
        string title,
        string message, 
        NotificationInformationType notificationInformationType)
    {
        try
        {
           await _notificationService.SendMailNotificationAsync(user, _mailNotificationsFactory.CreateNotification(notificationInformationType, title, message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during notification: {Message}", ex.Message);
        }
    }
    #endregion
}