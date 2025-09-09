using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface IFrequencyRepository
{
    Task<List<Frequency>> GetAllFrequencies();
    Task<Frequency?> GetFrequencyById(int id);
    Task<Frequency> CreateFrequency(Frequency frequency);
    Task<Frequency?> UpdateFrequency(Frequency frequency);
    Task<bool> DeleteFrequency(int id);
    
    // Frequency Detail methods
    Task<List<FrequencyDetail>> GetFrequencyDetailsByFrequencyId(int frequencyId);
    Task<FrequencyDetail> CreateFrequencyDetail(FrequencyDetail frequencyDetail);
    Task<bool> DeleteFrequencyDetail(int id);
}
