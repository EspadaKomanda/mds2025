using MDSBackend.Models;

namespace MDSBackend.Services.CurrentUsers;

public interface ICurrentUserService
{
    UserSession GetCurrentUser();
}
