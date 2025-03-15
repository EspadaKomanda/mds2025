using MDSBackend.Models;

namespace MDSBackend.Services.CurrentUserServiceNamespace;

public interface ICurrentUserService
{
    UserSession GetCurrentUser();
}
