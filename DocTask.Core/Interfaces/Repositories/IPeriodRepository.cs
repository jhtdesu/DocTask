using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface IPeriodRepository
{
    Task<List<Period>> GetAllPeriods();
    Task<Period?> GetPeriodById(int id);
    Task<Period> CreatePeriod(Period period);
    Task<Period?> UpdatePeriod(Period period);
    Task<bool> DeletePeriod(int id);
}
