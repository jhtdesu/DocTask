using DocTask.Core.Dtos.Progress;

namespace DocTask.Core.Interfaces.Services;

public interface IProgressService
{
    Task<List<ProgressDto>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null);
    Task<ProgressDto?> GetProgressById(int id);
}
