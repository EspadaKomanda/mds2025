using PurpleHackBackend.Models;

namespace PurpleHackBackend.Services.User;

public interface ICurrentUserService
{
    UserSession GetCurrentUser();
}