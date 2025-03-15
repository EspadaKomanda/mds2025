using MDSBackend.Models;

namespace MDSBackend.Services.CurrentUsers;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ICurrentUserService> _logger;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<ICurrentUserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public UserSession GetCurrentUser()
    {
        UserSession currentUser = new UserSession
        {
            IsAuthenticated = _httpContextAccessor.HttpContext.User.Identity != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
            Login = _httpContextAccessor.HttpContext.User.Identity.Name
        };
        _logger.LogDebug($"Current user extracted: {currentUser.Login}");
        return currentUser;
    }

}
