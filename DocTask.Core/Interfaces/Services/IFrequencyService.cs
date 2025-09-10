using DocTask.Core.Dtos.Frequency;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Services;

public interface IFrequencyService
{
    Task<List<FrequencyDto>> GetAllFrequencies();
    Task<PaginationResponse<FrequencyDto>> GetFrequenciesPaginated(PaginationRequest request);
    Task<FrequencyDto?> GetFrequencyById(int id);
    Task<FrequencyDto> CreateFrequency(CreateFrequencyRequest request);
    Task<FrequencyDto?> UpdateFrequency(int id, CreateFrequencyRequest request);
    Task<bool> DeleteFrequency(int id);
    
    // Frequency Detail methods
    Task<List<FrequencyDetailDto>> GetFrequencyDetailsByFrequencyId(int frequencyId);
    Task<PaginationResponse<FrequencyDetailDto>> GetFrequencyDetailsPaginated(int frequencyId, PaginationRequest request);
    Task<FrequencyDetailDto> CreateFrequencyDetail(CreateFrequencyDetailRequest request);
    Task<bool> DeleteFrequencyDetail(int id);
    
    // Combined frequency and periods creation
    Task<CreateFrequencyWithPeriodsResponse> CreateFrequencyWithPeriods(CreateFrequencyWithPeriodsRequest request);
}
