using MDSBackend.Models.Database;

namespace MDSBackend.Models.DTO;

public class GetAllRightsResponse
{
    public List<Right> Rights { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}