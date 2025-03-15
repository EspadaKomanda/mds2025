using MDSBackend.Models.DTO;

namespace MDSBackend.Services.Users;

public interface IUserService
{
    Task<bool> AddUser(UserDTO user);
    Task<UserDTO> GetUserById(long id);
    UserDTO GetUserByUsername(string username);
    IQueryable<UserDTO> GetAllUsers();
    Task<bool> UpdateUser(UserDTO user);
    bool DeleteUser(long id);
}
