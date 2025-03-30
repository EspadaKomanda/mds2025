using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.UsersProfile;

public interface IUserProfileService
{
    public Task<UserProfileDTO> AddUserProfile(long userId, UserProfileCreateDTO userProfile);
    public Task<UserProfile> AddUserProfile(UserProfile userProfile);
    public UserProfile? GetUserProfileByUserId(long id);
    public UserProfile? GetUserProfileById(long id);
    public Task<bool> UpdateUserProfileByUserId(long userId, UserProfileCreateDTO userProfile);
    public Task<bool> UpdateUserProfile(UserProfile userProfile);
    public bool DeleteUserProfile(long id);
}
