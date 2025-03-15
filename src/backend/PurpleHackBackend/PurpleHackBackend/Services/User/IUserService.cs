using PurpleHackBackend.Models.DTO;

namespace PurpleHackBackend.Services.UserServiceNamespace;

public interface IUserService
{
    Task<bool> AddUser(UserDTO user);
    Task<UserDTO> GetUserById(long id);
    UserDTO GetUserByUsername(string username);
    IQueryable<UserDTO> GetAllUsers();
    Task<bool> UpdateUser(UserDTO user);
    bool DeleteUser(long id);
}
