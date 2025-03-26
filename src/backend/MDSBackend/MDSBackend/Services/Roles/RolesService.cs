using MDSBackend.Database.Repositories;
using MDSBackend.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MDSBackend.Services.Roles;

public class RolesService : IRolesService
{
    #region Services

    private readonly ILogger<IRolesService> _logger;
    private readonly UnitOfWork _unitOfWork;

    #endregion
    
    #region Constructor

    public RolesService(ILogger<IRolesService> logger, UnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    #endregion
    
    #region Methods
    //TODO: refactor database work, to be more beautiful
    //ToDo: make better exception handling
    public async Task<ApplicationRole> CreateRoleAsync(string roleName, string description)
    {
        var role = new ApplicationRole(roleName)
        {
            Description = description
        };
        
        await _unitOfWork.RoleRepository.InsertAsync(role);
        if (await _unitOfWork.SaveAsync())
        {
            return role;
        }
        throw new Exception("Unable to create role");
    }

    public async Task UpdateRoleAsync(long roleId, string newRoleName, string newDescription)
    {
        var role = await _unitOfWork.RoleRepository.GetByIDAsync(roleId);
        
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {roleId} not found");
        }
        
        role.Name = newRoleName;
        role.Description = newDescription;

        if (!await _unitOfWork.SaveAsync())
        {
            throw new Exception("Unable to create role");
        }
    }

    public async Task DeleteRoleAsync(long roleId)
    {
        var role = await _unitOfWork.RoleRepository.GetByIDAsync(roleId);
        
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {roleId} not found");
        }
        
        _unitOfWork.RoleRepository.Delete(role);
        if (!await _unitOfWork.SaveAsync())
        {
            throw new Exception("Unable to delete role");
        }
    }

    public async Task AddRightToRoleAsync(long roleId, long rightId)
    {
        var role = await _unitOfWork.RoleRepository.Get()
            .Include(r => r.RoleRights)
            .FirstOrDefaultAsync(r => r.Id == roleId);
        
        var right = await _unitOfWork.RightRepository.GetByIDAsync(rightId);
        
        if (role == null || right == null)
        {
            throw new KeyNotFoundException($"Role or Right not found");
        }
        
        var existingRight = role.RoleRights.FirstOrDefault(rr => rr.RightId == rightId);

        if (existingRight == null)
        {
            role.RoleRights.Add(new RoleRight { RoleId = roleId, RightId = rightId });
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Unable to add role right");
            }
        }
    
    }

    public async Task RemoveRightFromRoleAsync(long roleId, long rightId)
    {
        var roleRight = await _unitOfWork.RoleRightRepository.Get()
            .FirstOrDefaultAsync(rr => rr.RoleId == roleId && rr.RightId == rightId);
        
        if (roleRight == null)
        {
            throw new KeyNotFoundException($"Right not found for role");
        }
        
        _unitOfWork.RoleRightRepository.Delete(roleRight);
        if (!await _unitOfWork.SaveAsync())
        {
            throw new Exception("Unable to remove role right");
        }
    }

    public async Task<ApplicationRole> GetRoleByIdAsync(long roleId)
    {
        return await _unitOfWork.RoleRepository.Get()
            .Include(r => r.RoleRights)
            .ThenInclude(rr => rr.Right)
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public async Task<(List<ApplicationRole> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _unitOfWork.RoleRepository.Get()
            .Include(r => r.RoleRights)
            .ThenInclude(rr => rr.Right);

        var totalItems = await query.CountAsync();
        var pagedRoles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (pagedRoles, totalItems);
    }
    #endregion

}