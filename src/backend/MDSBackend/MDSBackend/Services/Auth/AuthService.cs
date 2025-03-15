using AutoMapper;
using MDSBackend.Exceptions.Services.AuthService;
using MDSBackend.Exceptions.Services.User;
using MDSBackend.Models.DTO;
using MDSBackend.Services.Cookies;
using MDSBackend.Services.JWT;
using MDSBackend.Services.Users;

namespace MDSBackend.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthService> _logger;
    private readonly IJWTService _jwtService;
    private readonly ICookieService _cookieService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    
    public AuthService(IUserService userService, ILogger<AuthService> logger, IConfiguration configuration, IJWTService jwtService, ICookieService cookieService, IMapper mapper)
    {
        _userService = userService;
        _logger = logger;
        _jwtService = jwtService;
        _cookieService = cookieService;
        _mapper = mapper;
        _configuration = configuration;
    }

    /// <summary>
    /// Verifies a user's credentials and logs them in.
    /// </summary>
    /// <param name="user">The user to verify.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the user was logged in successfully; otherwise, false.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the user with the specified ID is not found.</exception>
    public async Task<string> Login(UserDTO user)
    {
        try
        {
            _logger.LogDebug("Attempting to login user {Username}", user.Username);
            var existingUser = _userService.GetUserByUsername(user.Username);
            if (existingUser == null)
            {
                _logger.LogWarning("User with username {Username} not found", user.Username);
                throw new UserNotFoundException($"User with username {user.Username} not found.");
            }

            var hashedPassword = existingUser.Password;
            if (!BCrypt.Net.BCrypt.Verify(user.Password, hashedPassword))
            {
                _logger.LogWarning("Incorrect password for user {Username}", user.Username);
                throw new InvalidDataException("Incorrect password for the specified user.");
            }
            var token = await _jwtService.GenerateJwtToken(existingUser);

            if (!await _cookieService.SetCookie("jwt", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["JwtSettings:TokenLifetime"]))
                }))
            {   
                _logger.LogWarning("Error setting cookie for user {Username}", user.Username);
                throw new InvalidOperationException($"Error setting cookie for user {user.Username}");
            }
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user {Username}", user.Username);
            throw new AuthServiceException(ex.Message);
        }
    }

    /// <summary>
    /// Creates a new user and logs them in.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the user was created and logged in successfully; otherwise, false.</returns>
    public async Task<string> Register(UserDTO user)
    {
        try
        {
            _logger.LogDebug("Attempting to register user {Username}", user.Username);
            var existingUser = _userService.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("User with username {Username} already exists", user.Username);
                throw new InvalidOperationException($"User with username {user.Username} already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            await _userService.AddUser(user);
            var token = await _jwtService.GenerateJwtToken(_mapper.Map<UserDTO>(user));
            if (! await _cookieService.SetCookie("jwt", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["JwtSettings:TokenLifetime"]))
                }))
            {
                _logger.LogWarning("Error setting cookie for user {Username}", user.Username);
                throw new InvalidOperationException($"Error setting cookie for user {user.Username}");
            }
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user {Username}", user.Username);
            throw new AuthServiceException(ex.Message);
        }
    }

    public async Task<bool> Logout()
    {
        try
        {
            _logger.LogDebug("Attempting to logout user");
            return await _cookieService.RemoveCookie("jwt");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out user");
            throw new AuthServiceException(ex.Message);
        }
    }
}
