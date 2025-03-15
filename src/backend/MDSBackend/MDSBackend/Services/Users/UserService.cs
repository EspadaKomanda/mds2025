using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.User;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.Users;

public class UserService : IUserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    public UserService(UnitOfWork unitOfWork, ILogger<UserService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    // TODO: this should return the user id instead (I think)
    public async Task<bool> AddUser(UserDTO user)
    {
        User UserEntity = _mapper.Map<User>(user);

       await _unitOfWork.BeginTransactionAsync();

        // Make sure a user with the given username does not exist yet
        if (_unitOfWork.UserRepository.Get(x => x.Username == user.Username).Any())
        {
            _logger.LogWarning("A user already exists with the given username: {Username}", user.Username);
            return false;
        }

        // Make sure a user with the given email does not exist yet
        if (_unitOfWork.UserRepository.Get(x => x.Email == user.Email).Any())
        {
            _logger.LogWarning("A user already exists with the given email: {Email}", user.Email);
            return false;
        }

        _unitOfWork.UserRepository.Insert(UserEntity);
        _unitOfWork.Save();
        await _unitOfWork.CommitAsync();
        return true;
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the UserDTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the user with the specified ID is not found.</exception>
    public async Task<UserDTO> GetUserById(long id)
    {
        try
        {
            _logger.LogDebug("Attempting to retrieve user with ID {UserId}", id);
            var user = await _unitOfWork.UserRepository.GetByIDAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                throw new UserNotFoundException($"User with ID {id} not found.");
            }

            _logger.LogDebug("User with ID {UserId} retrieved successfully", id);
            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            throw new UserServiceException(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the UserDTO.</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user with the specified username is not found.</exception>
    public UserDTO GetUserByUsername(string username)
    {
        try
        {
            _logger.LogDebug("Attempting to retrieve user with username {Username}", username);
            var user =  _unitOfWork.UserRepository.Get(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                _logger.LogWarning("User with username {Username} not found", username);
                throw new UserNotFoundException($"User with username {username} not found.");
            }

            _logger.LogDebug("User with username {Username} retrieved successfully", username);
            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with username {Username}", username);
            throw new UserServiceException(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A queryable of UserDTO.</returns>
    public IQueryable<UserDTO> GetAllUsers()
    {
        try
        {
            _logger.LogDebug("Attempting to retrieve all users");
            var users = _unitOfWork.UserRepository.Get();
            _logger.LogDebug("All users retrieved successfully");
            return (IQueryable<UserDTO>)users.Select(user => _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw new UserServiceException(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the user was updated successfully; otherwise, false.</returns>
    public async Task<bool> UpdateUser(UserDTO user)
    {
        try
        {
            _logger.LogDebug("Attempting to update user with ID {UserId}", user.Id);
            var existingUser = await _unitOfWork.UserRepository.GetByIDAsync(user.Id ?? throw new ArgumentNullException(nameof(user.Id)));
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", user.Id);
                throw new UserNotFoundException($"User with ID {user.Id} not found.");
            }

            _mapper.Map(user, existingUser);
            _unitOfWork.UserRepository.Update(existingUser);
            _unitOfWork.Save();
            _logger.LogDebug("User with ID {UserId} updated successfully", user.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", user.Id);
            throw new UserServiceException(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the user was deleted successfully; otherwise, false.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the user with the specified ID is not found.</exception>
    public bool DeleteUser(long id)
    {
        try
        {
            _logger.LogDebug("Attempting to delete user with ID {UserId}", id);
            _unitOfWork.UserRepository.Delete(id);
            _unitOfWork.Save();
            _logger.LogDebug("User with ID {UserId} deleted successfully", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
            throw new UserServiceException(ex.Message);
        }
    }

 
}
