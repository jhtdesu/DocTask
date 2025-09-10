using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Services;

public interface IUploadFileService
{
    Task<UploadFileDto> UploadFileAsync(UploadFileRequest request, string userId);
    Task<UploadFileDto?> GetFileByIdAsync(int fileId);
    Task<List<UploadFileDto>> GetFilesByUserIdAsync(string userId);
    Task<PaginationResponse<UploadFileDto>> GetFilesByUserIdPaginated(string userId, PaginationRequest request);
    Task<bool> DeleteFileAsync(int fileId, string userId);
    Task<byte[]?> GetFileContentAsync(int fileId);
}
