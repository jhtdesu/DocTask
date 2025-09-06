using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface IUploadFileRepository
{
    Task<Uploadfile> CreateAsync(Uploadfile uploadFile);
    Task<Uploadfile?> GetByIdAsync(int fileId);
    Task<List<Uploadfile>> GetByUserIdAsync(string userId);
    Task<bool> DeleteAsync(int fileId);
    Task<Uploadfile?> GetByIdAndUserIdAsync(int fileId, string userId);
}
