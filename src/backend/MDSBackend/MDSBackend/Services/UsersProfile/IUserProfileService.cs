using MDSBackend.Models.DTO;

namespace MDSBackend.Services.UsersProfile;

public interface IUserProfileService
{
    public Task<bool> AddUserProfile(UserProfileDTO userProfile);
    public UserProfileDTO GetUserProfileByUserId(long id);
    public UserProfileDTO GetUserProfileById(long id);
    public Task<bool> UpdateUserProfile(UserProfileDTO userProfile);
    public bool DeleteUserProfile(long id);
}
