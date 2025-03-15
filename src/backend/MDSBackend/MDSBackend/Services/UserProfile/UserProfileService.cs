using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.UserProfileServiceNamespace;

public class UserProfileService : IUserProfileService
{
private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<UserProfileService> _logger;
    private readonly IMapper _mapper;

    public UserProfileService(UnitOfWork unitOfWork, ILogger<UserProfileService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<bool> AddUserProfile(UserProfileDTO userProfile)
    {

        UserProfile userProfileEntity = _mapper.Map<UserProfile>(userProfile);

        await _unitOfWork.BeginTransactionAsync();

        // Make sure a user profile for the given user does not exist yet
        if (_unitOfWork.UserProfileRepository.Get(x => x.UserId == userProfile.UserId).Any())
        {
            _logger.LogWarning("A user profile already exists for the given user id: {UserId}", userProfile.UserId);
            return false;
        }

        await _unitOfWork.UserProfileRepository.InsertAsync(userProfileEntity);
        _unitOfWork.Save();
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("User profile added for user id: {UserId}", userProfile.UserId);
        return true;
    }
    public UserProfileDTO GetUserProfileByUserId(long id)
    {
        throw new NotImplementedException();
    }
    public UserProfileDTO GetUserProfileById(long id)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdateUserProfile(UserProfileDTO userProfile)
    {
        throw new NotImplementedException();
    }
    public bool DeleteUserProfile(long id)
    {
        throw new NotImplementedException();
    }
}
