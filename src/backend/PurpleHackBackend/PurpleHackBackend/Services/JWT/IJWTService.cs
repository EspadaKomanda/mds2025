using PurpleHackBackend.Models.DTO;

namespace PurpleHackBackend.Services.JWT;

public interface IJWTService
{ 
    Task<string> GenerateJwtToken(UserDTO user);
}