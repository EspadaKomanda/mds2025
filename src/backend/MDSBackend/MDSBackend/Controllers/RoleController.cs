using MDSBackend.Models.BasicResponses;
using MDSBackend.Models.DTO;
using MDSBackend.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Admin")]
public class RoleController : ControllerBase
{
    #region Fields

    private readonly IRolesService _rolesService;
    private readonly ILogger<RoleController> _logger;

    #endregion

    #region Constructor
    
    public RoleController(ILogger<RoleController> logger, IRolesService rolesService)
    {
        _logger = logger;
        _rolesService = rolesService;
    }

    #endregion

    #region ControllerMethods

    [HttpGet]
    public async Task<IActionResult> GetAllRolesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var (roles, totalCount) = await _rolesService.GetAllRolesAsync(pageNumber, pageSize);
            var response = new
            {
                Data = roles,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,ex.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to get roles"
            });
        }
       
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleByIdAsync(long id)
    {
        var role = await _rolesService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound(new BasicResponse()
            {
                
                Code = 404,
                Message = "Role not found"
                
            });
        }
        return Ok(role);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleDTO model)
    {
        try
        {

            var role = await _rolesService.CreateRoleAsync(model.Name, model.Description);
            return CreatedAtAction(nameof(GetRoleByIdAsync), new { id = role.Id }, role);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to create role"
            });
        }
    }
    
    

    
    #endregion
}