using MDSBackend.Models.DTO;

namespace MDSBackend.Services.Auth;

public interface IAuthService
{
    Task<string> Login(UserDTO user);
    Task<string> Register(UserDTO user);
    Task<bool> Logout();
}