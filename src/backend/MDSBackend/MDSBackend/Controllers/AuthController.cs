using MDSBackend.Models.BasicResponses;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.JWT;
using MDSBackend.Services.NotificationService;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;
using Microsoft.AspNetCore.Authorization;
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
    private readonly INotificationService _notificationService;
    private readonly MailNotificationsFactory _mailNotificationsFactory;
    private readonly PushNotificationsFactory _pushNotificationsFactory;
    
    
    #endregion
    
    #region Constructor

    public AuthController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService, INotificationService notificationService, MailNotificationsFactory mailNotificationsFactory, PushNotificationsFactory pushNotificationsFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _notificationService = notificationService;
        _mailNotificationsFactory = mailNotificationsFactory;
        _pushNotificationsFactory = pushNotificationsFactory;
    }
    
    #endregion
    
    #region Actions

    #region Auth

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
                TwoFactorEnabled = false,
                TwoFactorProviders = new List<TwoFactorProvider>() { TwoFactorProvider.NONE }
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
                return NotFound(new BasicResponse
                {
                    Code = 404,
                    Message = "Invalid username or password"
                });
            }
            var result =  await _signInManager.PasswordSignInAsync( user, model.Password, false, model.RememberMe);

            if (result.Succeeded & !result.RequiresTwoFactor)
            {
                var refreshToken = await _jwtService.GenerateRefreshTokenAsync(user);
                var accessToken = _jwtService.GenerateAccessToken(user);
                _logger.LogInformation("User logged in successfully: {Username}", model.Username);

                return Ok(new LoginResultResponse()
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
            else if(result.RequiresTwoFactor)
            {
                var providerWithMaxWeight = user.TwoFactorProviders
                    .OrderByDescending(p => (int)p) 
                    .FirstOrDefault();

                if (providerWithMaxWeight == TwoFactorProvider.NONE)
                {
                    _logger.LogInformation("User {Username} does not have any two-factor authentication enabled", model.Username);
                    return StatusCode(418, new LoginResultResponse()
                    {
                        RequiresTwoFactorAuth = false,
                        Success = true,
                        TwoFactorProvider = (int)providerWithMaxWeight
                    });
                }

                var code = await _userManager.GenerateTwoFactorTokenAsync(user, providerWithMaxWeight.ToString());
                await SendNotificationAsync(user, "Two-factor authentication code", code, NotificationInformationType.AUTH,providerWithMaxWeight);
                _logger.LogInformation("Two-factor authentication required for user {Username}", model.Username);
                return Ok(new LoginResultResponse()
                {
                    RequiresTwoFactorAuth = true,
                    Success = true
                });
            }
            
            _logger.LogError("Invalid username or password");
            return BadRequest(new BasicResponse
            {
                Code = 400,
                Message = "Invalid username or password"
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
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged out successfully");
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found");
        }

        await _jwtService.RevokeRefreshTokenAsync(user.Id, HttpContext.Request.Cookies["refresh_token"],GetRemoteIpAddress());
        return Ok("Token revoked successfully");
    }
    #endregion

    #region Email

    [HttpGet("{username}/init-email-verification")]
    public async Task<IActionResult> VerifyEmail(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogError("Invalid username or password");
                return NotFound(new BasicResponse
                {
                    Code = 404,
                    Message = "Invalid username or password"
                });
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendNotificationAsync(user, "Email verification code", code, NotificationInformationType.AUTH, TwoFactorProvider.EMAIL);
            _logger.LogInformation("Email verification code sent to user {Username}", username);
            return Ok(new BasicResponse()
            {
                Code = 200,
                Message = "Email verification code sent"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred during email verification: {Message}", e.Message);
            return StatusCode(500, new BasicResponse
            {
                Code = 500,
                Message = "An error occurred during email verification"
            });
        }
    }
    
    [HttpGet("{username}/verify-email/{code}")]
    public async Task<IActionResult> VerifyEmail(string username, string code)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogError("Invalid username or password");
                return NotFound(new BasicResponse
                {
                    Code = 404,
                    Message = "Invalid username or password"
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user,code);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email verified for user {Username}", username);
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Email verified"
                });
            }
            return BadRequest(new BasicResponse()
            {
                Code = 400,
                Message = "Email verification failed"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred during email verification: {Message}", e.Message);
            return StatusCode(500, new BasicResponse
            {
                Code = 500,
                Message = "An error occurred during email verification"
            });
        }
    }
    #endregion
    
    #region 2FA

    [HttpPost("get-2fa-code")]
    public async Task<IActionResult> GetTwoFactorCode([FromBody] GetTwoFactorDTO model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user == null)
            {
                _logger.LogError("Invalid username or password");
                return NotFound(new BasicResponse
                {
                    Code = 404,
                    Message = "Invalid username or password"
                });
            }
            
            var providerWithRequiredWeight = user.TwoFactorProviders
                .FirstOrDefault(p => (int)p == model.TwoFactorProvider);
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, providerWithRequiredWeight.ToString());
            await SendNotificationAsync(user, "Two-factor authentication code", code, NotificationInformationType.AUTH,providerWithRequiredWeight);
            _logger.LogInformation("Two-factor authentication code sent to user {Username}", model.Username);
            
            return Ok(new BasicResponse()
            {
                Code = 200,
                Message = "Code sent successfully"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to send code"
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
                var providerWithRequiredWeight = user.TwoFactorProviders
                    .FirstOrDefault(p => (int)p == model.TwoFactorProvider);
                var signInResult = _signInManager.TwoFactorSignInAsync(providerWithRequiredWeight.ToString(),
                    model.Code, false, model.RememberMe);
                
                
                if (!signInResult.Result.Succeeded)
                {
                    return BadRequest(new BasicResponse()
                    {
                        Code = 400,
                        Message = "Invalid code"
                    });
                }
                
                var token = _jwtService.GenerateAccessToken(user);
                var refreshToken =  await _jwtService.GenerateRefreshTokenAsync(user);

                _logger.LogInformation("User logged in successfully: {Username}", model.Username);
                await SendNotificationAsync(user, "Login successful", "You have successfully logged in", NotificationInformationType.WARNING,TwoFactorProvider.EMAIL);
                
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
            return NotFound(new BasicResponse()
            {
                Code = 404,
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

    [HttpPost("enable-2fa")]
    [Authorize]
    public async Task<IActionResult> EnableTwoFactor([FromBody]EnableTwoFactorDTO model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "User not found"
            });
        }
        
        user.TwoFactorProviders.Add((TwoFactorProvider)model.TwoFactorProvider);
        user.TwoFactorEnabled = true;
        await _userManager.UpdateAsync(user);
        var secretKey = await _userManager.GenerateTwoFactorTokenAsync(user, TwoFactorProvider.AUTHENTICATOR.ToString());
        _logger.LogInformation("User logged in successfully: {Username}", User);
        await SendNotificationAsync(user, "Login successful", "You have successfully logged in", NotificationInformationType.WARNING,(TwoFactorProvider)model.TwoFactorProvider);

        return Ok(new BasicResponse()
        {
            Code = 200,
            Message = "Two-factor authentication enabled successfully"
        });
    }
    
    [HttpPost("disable-2fa")]
    [Authorize]
    public async Task<IActionResult> DisableTwoFactor([FromBody] DisableTwoFactorDTO model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (!await _userManager.VerifyTwoFactorTokenAsync(user,model.TwoFactorProvider.ToString(),model.Code))
        {
            return BadRequest("Invalid verification code");
        }

        user.TwoFactorEnabled = false;
        await _userManager.UpdateAsync(user);

        return Ok("Two-factor authentication disabled");
    }

    #endregion
    
    #region Helpers

    private async Task SendNotificationAsync(ApplicationUser user, 
        string title,
        string message, 
        NotificationInformationType notificationInformationType,
        TwoFactorProvider provider)
    {
        try
        {
            switch (provider)
            {
                case TwoFactorProvider.EMAIL:
                    await _notificationService.SendMailNotificationAsync(user, _mailNotificationsFactory.CreateNotification(notificationInformationType, title, message));
                    break;
                case TwoFactorProvider.PHONE:
                    throw new NotImplementedException();
                    break;
                case TwoFactorProvider.PUSH:                    
                    await _notificationService.SendPushNotificationAsync(user, _pushNotificationsFactory.CreateNotification(notificationInformationType, title, message));
                    break;
                case TwoFactorProvider.AUTHENTICATOR:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
         }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during notification: {Message}", ex.Message);
        }
    }
    
    private string GetRemoteIpAddress()
    {
        if (HttpContext.Connection.RemoteIpAddress != null)
        {
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        return string.Empty;
    }
    #endregion
    
    #endregion
}