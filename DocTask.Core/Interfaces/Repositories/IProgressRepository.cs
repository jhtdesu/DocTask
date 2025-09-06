using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface IProgressRepository
{
    Task<List<Progress>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null);
    Task<Progress?> GetProgressById(int id);
    Task<Progress> CreateProgress(Progress progress);
    Task<Progress?> UpdateProgress(Progress progress);
    Task<bool> DeleteProgress(int id);
    Task<bool> ProgressExists(int id);
}
