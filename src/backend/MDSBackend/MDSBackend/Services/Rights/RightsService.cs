using MDSBackend.Database.Repositories;
using MDSBackend.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MDSBackend.Services.Rights;

public class RightsService : IRightsService
{
    #region Fields
    
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<IRightsService> _logger;
    
    #endregion
    
    #region Constructor

    public RightsService(UnitOfWork unitOfWork, ILogger<IRightsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    #endregion
    
    #region Methods
    
    public async Task<Right?> CreateRightAsync(string rightName, string description)
    {
        var right = new Right
        {
            Name = rightName,
            Description = description
        };
        
        await _unitOfWork.RightRepository.InsertAsync(right);
        if (await _unitOfWork.SaveAsync())
        {
            return right;
        }
        
        throw new Exception($"Unable to create right for {rightName}");
    }

    public async Task UpdateRightAsync(long rightId, string newRightName, string newDescription)
    {
        var right = await _unitOfWork.RightRepository.GetByIDAsync(rightId);
        
        if (right == null)
        {
            throw new KeyNotFoundException($"Right with ID {rightId} not found");
        }
        
        right.Name = newRightName;
        right.Description = newDescription;

        if (!await _unitOfWork.SaveAsync())
        {
            throw new Exception($"Unable to create right for {rightId}");
        }
    }

    public async Task DeleteRightAsync(long rightId)
    {
        var right = await _unitOfWork.RightRepository.GetByIDAsync(rightId);
        
        if (right == null)
        {
            throw new KeyNotFoundException($"Right with ID {rightId} not found");
        }
        
        _unitOfWork.RightRepository.Delete(right);
        if (!await _unitOfWork.SaveAsync())
        {
            throw new Exception($"Unable to delete right for {rightId}");
        }
    }

    public async Task<Right?> GetRightByIdAsync(long rightId)
    {
        return await _unitOfWork.RightRepository.GetByIDAsync(rightId);
    }

    public async Task<(List<Right> Rights, int TotalCount)> GetAllRightsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _unitOfWork.RightRepository.Get();
    
        var totalItems = await query.CountAsync();
    
        // Apply pagination
        var pagedRights = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    
        return (pagedRights, totalItems);
    }
    
    #endregion
}