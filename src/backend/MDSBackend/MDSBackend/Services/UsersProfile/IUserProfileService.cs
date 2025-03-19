using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.UsersProfile;

public interface IUserProfileService
{
    // XXX: Do we really need two of these methods?
    public Task<UserProfile> AddUserProfile(UserProfileDTO userProfile);
    public Task<UserProfile> AddUserProfile(UserProfile userProfile);
    public UserProfile? GetUserProfileByUserId(long id);
    public UserProfile? GetUserProfileById(long id);
    // XXX: Are model/DTO methods necessary?
    public Task<bool> UpdateUserProfile(UserProfileDTO userProfile);
    public Task<bool> UpdateUserProfile(UserProfile userProfile);
    public bool DeleteUserProfile(long id);
}
