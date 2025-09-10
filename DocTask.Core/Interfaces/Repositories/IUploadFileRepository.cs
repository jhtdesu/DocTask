using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Repositories;

public interface IUploadFileRepository
{
    Task<Uploadfile> CreateAsync(Uploadfile uploadFile);
    Task<Uploadfile?> GetByIdAsync(int fileId);
    Task<List<Uploadfile>> GetByUserIdAsync(string userId);
    Task<(List<Uploadfile> items, int totalCount)> GetByUserIdPaginated(string userId, PaginationRequest request);
    Task<bool> DeleteAsync(int fileId);
    Task<Uploadfile?> GetByIdAndUserIdAsync(int fileId, string userId);
}
