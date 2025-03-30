using AutoMapper;
using MDSBackend.Exceptions.Services.ProfileService;
using MDSBackend.Models.DTO;
using MDSBackend.Services.UsersProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
[Authorize(Policy = "User")]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfilesService;
    private readonly ILogger<UserProfileController> _logger;
    private readonly IMapper _mapper;

    public UserProfileController(IUserProfileService userProfilesService, ILogger<UserProfileController> logger, IMapper mapper)
    {
        _userProfilesService = userProfilesService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a user profile by its ID.
    /// </summary>
    /// <param name="id">The ID of the user profile.</param>
    /// <returns>An <see cref="UserProfileDTO"/> containing the user profile DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the user profile DTO</response>
    /// <response code="404">If the user profile is not found</response>
    [HttpGet("user/{id}")]
    public IActionResult GetUserProfileByUserId(long id)
    {
        try
        {
            var userProfile = _userProfilesService.GetUserProfileByUserId(id);
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
    /// <param name="model">The user profile model.</param>
    /// <returns>A <see cref="UserProfileDTO"/> containing the created user profile if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="201">Returns the created user profile</response>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddUserProfile([FromBody] UserProfileDTO model)
    {
        try
        {
            var userProfile = await _userProfilesService.AddUserProfile(model);
            return Ok(_mapper.Map<UserProfileDTO>(userProfile));
        }
        catch (ProfileNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates an existing user profile.
    /// </summary>
    /// <param name="model">The user profile model.</param>
    /// <returns>A <see cref="UserProfileDTO"/> containing the updated user profile if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the updated user profile</response>
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO model)
    {
        // TODO: verify admin access / user ownership
        try
        {
            var userProfile = await _userProfilesService.UpdateUserProfile(model);
            return Ok(_mapper.Map<UserProfileDTO>(userProfile));
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

