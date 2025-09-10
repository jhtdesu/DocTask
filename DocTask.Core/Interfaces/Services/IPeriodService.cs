using DocTask.Core.Dtos.Period;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Services;

public interface IPeriodService
{
    Task<List<PeriodDto>> GetAllPeriods();
    Task<PaginationResponse<PeriodDto>> GetPeriodsPaginated(PaginationRequest request);
    Task<PeriodDto?> GetPeriodById(int id);
    Task<PeriodDto> CreatePeriod(CreatePeriodRequest request);
    Task<PeriodDto?> UpdatePeriod(int id, CreatePeriodRequest request);
    Task<bool> DeletePeriod(int id);
}
