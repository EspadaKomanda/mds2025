using PurpleHackBackend.Models;

namespace PurpleHackBackend.Services.CurrentUserServiceNamespace;

public interface ICurrentUserService
{
    UserSession GetCurrentUser();
}
