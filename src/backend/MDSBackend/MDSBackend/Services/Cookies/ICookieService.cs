namespace MDSBackend.Services.Cookies;

public interface ICookieService
{ 
    Task<bool> SetCookie(string key, string value, CookieOptions options);
    Task<bool> RemoveCookie(string key);
}