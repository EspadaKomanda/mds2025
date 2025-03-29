using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.ProfileService;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.UsersProfile;

public class UserProfileService : IUserProfileService
{
private readonly UnitOfWork _unitOfWork;

    # region Services

    private readonly ILogger<UserProfileService> _logger;
    private readonly IMapper _mapper;

    #endregion


    #region Constructor

    public UserProfileService(UnitOfWork unitOfWork, ILogger<UserProfileService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    #endregion

    # region Methods

    public async Task<UserProfile> AddUserProfile(UserProfileDTO userProfile)
    {
        UserProfile userProfileEntity = _mapper.Map<UserProfile>(userProfile);

        return await AddUserProfile(userProfileEntity);
    }

    public async Task<UserProfile> AddUserProfile(UserProfile userProfile)
    {

        UserProfile userProfileEntity = userProfile;

        // Make sure a user profile for the given user does not exist yet
        if (_unitOfWork.UserProfileRepository.Get(x => x.UserId == userProfile.UserId).Any())
        {
            _logger.LogWarning("A user profile already exists for the given user id: {UserId}", userProfile.UserId);
            throw new ProfileExistsException($"{userProfile.UserId}");
        }

        await _unitOfWork.UserProfileRepository.InsertAsync(userProfileEntity);
        if (await _unitOfWork.SaveAsync())
        {
            _logger.LogInformation("User profile added for user id: {UserId}", userProfile.UserId);
            return userProfileEntity;
        }

        _logger.LogError("Failed to add user profile for user id: {UserId}", userProfile.UserId);
        throw new ProfileCreationException();
    }

    public UserProfile? GetUserProfileByUserId(long id)
    {
        return _unitOfWork.UserProfileRepository.Get(x => x.UserId == id).FirstOrDefault();
    }

    public UserProfile? GetUserProfileById(long id)
    {
        return _unitOfWork.UserProfileRepository.GetByID(id);
    }

    public async Task<bool> UpdateUserProfile(UserProfileDTO userProfile)
    {
        var userProfileEntityUpdated = _mapper.Map<UserProfile>(userProfile);
        return await UpdateUserProfile(userProfileEntityUpdated);
    }

    public async Task<bool> UpdateUserProfile(UserProfile userProfile)
    {
        var userProfileEntityUpdated = userProfile;
        var userProfileEntity = await _unitOfWork.UserProfileRepository.GetByIDAsync(userProfileEntityUpdated.Id);

        if (userProfileEntity == null)
        {
            throw new ProfileNotFoundException($"{userProfileEntityUpdated.Id}");   
        }
        
        // TODO: make sure that the mapper will act as intended
        _mapper.Map(userProfileEntityUpdated, userProfileEntity);

        if (!await _unitOfWork.SaveAsync())
        {
            throw new ProfileUpdateException($"Failed to update user profile {userProfileEntityUpdated.Id}");
        }

        _logger.LogInformation("User profile updated for user id: {UserId}", userProfile.UserId);
        return true; 
    }

    public bool DeleteUserProfile(long id)
    {
        var profile = _unitOfWork.UserProfileRepository.GetByID(id);
        if (profile == null)
        {
            throw new ProfileNotFoundException($"{id}");
        }

        _unitOfWork.UserProfileRepository.Delete(id);
        if (_unitOfWork.Save())
        {
            _logger.LogInformation("User profile deleted: {UserId}", id);
            return true;
        }
        throw new ProfileDeletionException($"Failed to delete user profile {id}"); 
    }

    #endregion
}
