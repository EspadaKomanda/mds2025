using MDSBackend.Models.DTO;

namespace MDSBackend.Services.JWT;

public interface IJWTService
{ 
    Task<string> GenerateJwtToken(UserDTO user);
}