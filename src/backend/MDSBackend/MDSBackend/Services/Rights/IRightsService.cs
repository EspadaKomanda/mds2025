using MDSBackend.Models.Database;

namespace MDSBackend.Services.Rights;

public interface IRightsService
{
    Task<Right?> CreateRightAsync(string rightName, string description);
    Task<bool> UpdateRightAsync(long rightId, string newRightName, string newDescription);
    Task<bool> DeleteRightAsync(long rightId);
    Task<Right?> GetRightByIdAsync(long rightId);
    Task<(List<Right> Rights, int TotalCount)> GetAllRightsAsync(int pageNumber = 1, int pageSize = 10);
}