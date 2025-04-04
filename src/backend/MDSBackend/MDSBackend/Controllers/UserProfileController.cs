using AutoMapper;
using MDSBackend.Exceptions.Services.ProfileService;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.UsersProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
[Authorize(Policy = "User")]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfilesService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserProfileController> _logger;
    private readonly IMapper _mapper;

    public UserProfileController(IUserProfileService userProfilesService, UserManager<ApplicationUser> userManager, ILogger<UserProfileController> logger, IMapper mapper)
    {
        _userProfilesService = userProfilesService;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a user profile by its ID.
    /// </summary>
    /// <param name="username">The username of the user profile's owner.</param>
    /// <returns>An <see cref="UserProfileDTO"/> containing the user profile DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the user profile DTO</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpGet("user/{username}")]
    public async Task<IActionResult> GetUserProfileByUsername(string username)
    {
        try
        {
            var user = (await _userManager.FindByNameAsync(username));
            if (user == null)
            {
                return NotFound();
            }

            var userProfile = _userProfilesService.GetUserProfileByUserId(user.Id);
            return Ok(_mapper.Map<UserProfileDTO>(userProfile));
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets a user profile by its ID.
    /// </summary>
    /// <param name="id">The ID of the user profile.</param>
    /// <returns>An <see cref="UserProfileDTO"/> containing the user profile DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the user profile DTO</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpGet("{id}")]
    public IActionResult GetUserProfileById(long id)
    {
        try
        {
            var userProfile = _userProfilesService.GetUserProfileById(id);
            return Ok(_mapper.Map<UserProfileDTO>(userProfile));
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Adds a new user profile.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="model">The user profile model.</param>
    /// <returns>A <see cref="UserProfileDTO"/> containing the created user profile if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the created user profile</response>
    /// <response code="404">If the user is not found</response>
    [HttpPost("user/{username}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddUserProfile(string username, [FromBody] UserProfileCreateDTO model)
    {
        var user = (await _userManager.FindByNameAsync(username));
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            var userProfile = await _userProfilesService.AddUserProfile(user.Id, model);
            return Ok(_mapper.Map<UserProfileDTO>(userProfile));
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update user profile for the logged in user.
    /// </summary>
    /// <param name="model">The user profile model.</param>
    /// <returns>A <see cref="UserProfileDTO"/> containing the updated user profile if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the updated user profile</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileCreateDTO model)
    {
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

        try
        { 
            bool result = await _userProfilesService.UpdateUserProfileByUserId(userId, model);
            return Ok(result);
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates an existing user profile.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="model">The user profile model.</param>
    /// <returns>A <see cref="UserProfileDTO"/> containing the updated user profile if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the updated user profile</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpPut]
    [Authorize(Policy = "Admin")]
    [Route("user/{userId}")]
    public async Task<IActionResult> UpdateUserProfileByUsername(string username, [FromBody] UserProfileCreateDTO model)
    {
        var user = (await _userManager.FindByNameAsync(username));
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            bool result = await _userProfilesService.UpdateUserProfileByUserId(user.Id, model);
            return Ok(result);
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes an existing user profile.
    /// </summary>
    /// <param name="id">The ID of the user profile to delete.</param>
    /// <returns>A <see cref="bool"/></returns>
    /// <response code="200">Returns true.</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult DeleteUserProfile(long id)
    {
        try
        {
            _userProfilesService.DeleteUserProfile(id);
            return Ok();
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

}

