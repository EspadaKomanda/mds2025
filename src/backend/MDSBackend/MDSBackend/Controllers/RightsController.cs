using MDSBackend.Services.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Admin")]
public class RightsController : ControllerBase
{
    #region Services

    private readonly IRightsService _rightsService;
    private readonly ILogger<RightsController> _logger;

    #endregion

    #region Constructor

    public RightsController(IRightsService rightsService, ILogger<RightsController> logger)
    {
        _rightsService = rightsService;
        _logger = logger;
    }

    #endregion

    #region Methods

    [HttpGet]
    public async Task<IActionResult> GetAllRightsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var (rights, totalCount) = await _rightsService.GetAllRightsAsync(pageNumber, pageSize);
        var response = new
        {
            Data = rights,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return Ok(response);
    }

    #endregion
}