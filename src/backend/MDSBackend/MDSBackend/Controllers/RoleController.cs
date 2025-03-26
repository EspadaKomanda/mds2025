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
    #region Services

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
            _logger.LogInformation($"Roles found successfully, {roles.Count}");
            var response = new GetAllRolesResponse()
            {
                Roles = roles,
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
        _logger.LogInformation($"Role found successfully, {role.Id}");
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
            _logger.LogInformation($"Role created successfully, {role.Id}");
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
    
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoleAsync(long id, [FromBody] RoleDTO model)
    {
        try
        {
            if (await _rolesService.UpdateRoleAsync(id, model.Name, model.Description))
            {
                _logger.LogInformation($"Role updated successfully, {id}");
            
                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Role updated successfully"
                });
            }
            
            _logger.LogCritical($"Unknown error with role updating, {id}");
            return StatusCode(418,new BasicResponse()
            {
                Code = 418,
                Message = "Role not found"
            });
           
        }
        catch (KeyNotFoundException)
        {
            _logger.LogError($"Role not found, {id} ");
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Role not found"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to update role"
            });
        }
       
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoleAsync(long id)
    {
        try
        {
            if (await _rolesService.DeleteRoleAsync(id))
            {

                _logger.LogInformation($"Role updated successfully, {id}");

                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Role updated successfully"
                });
            }

            _logger.LogCritical($"Unknown error with role deleting, RoleId {id}");
            return StatusCode(418,new BasicResponse()
            {
                Code = 418,
                Message = "Role not found"
            });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogError($"Role not found, {id} ");
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Role not found"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to update role"
            });
        }
       
    }
    
    
    [HttpPost("{roleId}/rights/{rightId}")]
    public async Task<IActionResult> AddRightToRoleAsync(long roleId, long rightId)
    {
        try
        {
            if (await _rolesService.AddRightToRoleAsync(roleId, rightId))
            {
                _logger.LogInformation($"Right added to role successfully, RoleId: {roleId}, RightId: {rightId}");
                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Right added to role successfully"
                });
            }
            
            _logger.LogCritical($"Unknown error with adding right to role, RoleId: {roleId}, RightId: {rightId}");
            return StatusCode(418,new BasicResponse()
            {
                Code = 418,
                Message = "Right not found for role"
            });
        }
        catch(KeyNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Right not found for role"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to add right to role"
            });
        }
    }

    [HttpDelete("{roleId}/rights/{rightId}")]
    public async Task<IActionResult> RemoveRightFromRoleAsync(long roleId, long rightId)
    {
        try
        {

            if (await _rolesService.RemoveRightFromRoleAsync(roleId, rightId))
            {
                _logger.LogInformation($"Right removed from role successfully, RoleId: {roleId}, RightId: {rightId}");

                return Ok(new BasicResponse()
                {
                    Code = 200,
                    Message = "Right removed from role successfully"
                });
            }

            _logger.LogCritical($"Unknown error with removing right from role, RoleId: {roleId}, RightId: {rightId}");
            return StatusCode(418, new BasicResponse()
            {
                Code = 418,
                Message = "Right not found right for role"
            });

        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(new BasicResponse()
            {
                Code = 404,
                Message = "Right not found for role"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new BasicResponse()
            {
                Code = 500,
                Message = "Failed to remove right from role"
            });
        }
    }
    
    
    
    #endregion
}