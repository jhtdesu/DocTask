using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Repositories;

public interface IPeriodRepository
{
    Task<List<Period>> GetAllPeriods();
    Task<(List<Period> items, int totalCount)> GetPeriodsPaginated(PaginationRequest request);
    Task<Period?> GetPeriodById(int id);
    Task<Period> CreatePeriod(Period period);
    Task<Period?> UpdatePeriod(Period period);
    Task<bool> DeletePeriod(int id);
}
