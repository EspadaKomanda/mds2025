using PurpleHackBackend.Models.DTO;

namespace PurpleHackBackend.Services.User;

public interface IUserService
{
    Task<bool> AddUser(UserDTO user);
    Task<UserDTO> GetUserById(long id);
    Task<UserDTO> GetUserByUsername(string username);
    IQueryable<UserDTO> GetAllUsers();
    Task<bool> UpdateUser(UserDTO user);
    bool DeleteUser(long id);
}