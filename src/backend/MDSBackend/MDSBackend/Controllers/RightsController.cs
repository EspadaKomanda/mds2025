using MDSBackend.Models.BasicResponses;
using MDSBackend.Models.DTO;
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
        try
        {
            var (rights, totalCount) = await _rightsService.GetAllRightsAsync(pageNumber, pageSize);
            
            _logger.LogInformation($"Retrieved {rights.Count} rights");
            
            var response = new GetAllRightsResponse()
            {
                Rights = rights,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to get rights.",
            });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRightByIdAsync(long id)
    {
        var right = await _rightsService.GetRightByIdAsync(id);
        _logger.LogInformation($"Retrieved right with id: {id}");
        if (right == null)
        {   
            return NotFound(new BasicResponse()
            {
                
                Code = 404,
                Message = "Right not found"
                
            });
        }
        return Ok(right);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRightAsync([FromBody] RightDTO model)
    {
        try
        {
            var right = await _rightsService.CreateRightAsync(model.Name, model.Description);
            
            _logger.LogInformation($"Created right: {right}");
            
            return CreatedAtAction(nameof(CreateRightAsync), new { id = right.Id }, right);

        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to create right.",
            });
        }
     }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRightAsync(long id, [FromBody] RightDTO model)
    {
        try
        {
            if (await _rightsService.UpdateRightAsync(id, model.Name, model.Description))
            {
                _logger.LogInformation($"Updated right: {id}");
                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Rights updated",
                });
            }
            _logger.LogError($"Unknown with right updating, {id}");
            return StatusCode(418,
                new BasicResponse()
                {
                    Code = 418,
                    Message = "Failed to update right."
                });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogError($"Right not found, {id} ");
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Right not found"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to update right"
            });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRightAsync(long id)
    {
        try
        {
            if( await _rightsService.DeleteRightAsync(id))
            {
                _logger.LogInformation($"Deleted right: {id}");
                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Rights deleted",
                });
            }
            _logger.LogError($"Unknown error with right deleting, {id} ");
            return StatusCode(418, new BasicResponse()
            {
                Code = 418,
                Message = "Failed to delete right"
            });

        }
        catch (KeyNotFoundException)
        {
            _logger.LogError($"Role not found, {id} ");
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Right not found"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to delete right"
            });
        }
    }
    
    
    #endregion
}