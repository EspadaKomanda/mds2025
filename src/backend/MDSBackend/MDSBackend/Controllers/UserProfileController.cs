using AutoMapper;
using MDSBackend.Services.UsersProfile;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
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
}

