using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Repositories;

public interface IProgressRepository
{
    Task<List<Progress>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null);
    Task<(List<Progress> items, int totalCount)> GetProgressPaginated(PaginationRequest request, DateTime? startDate = null, DateTime? dueDate = null);
    Task<Progress?> GetProgressById(int id);
    Task<Progress> CreateProgress(Progress progress);
    Task<Progress?> UpdateProgress(Progress progress);
    Task<bool> DeleteProgress(int id);
    Task<bool> ProgressExists(int id);
}
