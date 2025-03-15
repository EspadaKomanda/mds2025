using Microsoft.AspNetCore.Mvc;
using PurpleHackBackend.Models.DTO;
using PurpleHackBackend.Services.UserServiceNamespace;

namespace PurpleHackBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private IUserService _userService;
    public TestController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    [Route("test")]
    public async Task<string> Get()
    {
      // create a test user
      UserDTO user = new UserDTO()
      {
        Id = 228,
        Username = "test",
        Password = "test",
        Email = "test"
      };
      
      await _userService.AddUser(user);

      return "Hello World!";
    }
  }
}
