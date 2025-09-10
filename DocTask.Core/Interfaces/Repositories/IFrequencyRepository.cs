using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.Interfaces.Repositories;

public interface IFrequencyRepository
{
    Task<List<Frequency>> GetAllFrequencies();
    Task<(List<Frequency> items, int totalCount)> GetFrequenciesPaginated(PaginationRequest request);
    Task<Frequency?> GetFrequencyById(int id);
    Task<Frequency> CreateFrequency(Frequency frequency);
    Task<Frequency?> UpdateFrequency(Frequency frequency);
    Task<bool> DeleteFrequency(int id);
    
    // Frequency Detail methods
    Task<List<FrequencyDetail>> GetFrequencyDetailsByFrequencyId(int frequencyId);
    Task<(List<FrequencyDetail> items, int totalCount)> GetFrequencyDetailsPaginated(int frequencyId, PaginationRequest request);
    Task<FrequencyDetail> CreateFrequencyDetail(FrequencyDetail frequencyDetail);
    Task<bool> DeleteFrequencyDetail(int id);
}
