using DocTask.Core.Dtos.Progress;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Services;

public interface IProgressService
{
    Task<List<ProgressDto>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null);
    Task<PaginationResponse<ProgressDto>> GetProgressPaginated(PaginationRequest request, DateTime? startDate = null, DateTime? dueDate = null);
    Task<ProgressDto?> GetProgressById(int id);
    Task<ProgressDto> CreateProgress(CreateProgressDto createProgressDto, string updatedBy);
}
