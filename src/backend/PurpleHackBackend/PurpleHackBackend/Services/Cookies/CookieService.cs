using PurpleHackBackend.Exceptions.UtilServices.Cookies;

namespace PurpleHackBackend.Services.Cookies;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CookieService> _logger;
    private readonly IConfiguration _configuration;

    public CookieService(IHttpContextAccessor httpContextAccessor, ILogger<CookieService> logger, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _configuration = configuration;
    }

    public Task<bool> SetCookie(string key, string value, CookieOptions options)
    {
        try
        {
            _logger.LogDebug("Adding cookie {CookieKey} with value {CookieValue}", key, value);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add cookie {CookieKey}", key);
            throw new SetCookiesException(ex.Message);
        }
    }

    public async Task<bool> RemoveCookie(string key)
    {
        try
        {
            _logger.LogDebug("Deleting cookie {CookieKey}", key);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete cookie {CookieKey}", key);
            throw new DeleteCookiesException(ex.Message);
        }
    }
}