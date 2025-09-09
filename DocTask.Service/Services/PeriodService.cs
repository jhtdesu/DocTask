using DocTask.Core.Dtos.Period;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Service.Mappers;

namespace DocTask.Service.Services;

public class PeriodService : IPeriodService
{
    private readonly IPeriodRepository _periodRepository;

    public PeriodService(IPeriodRepository periodRepository)
    {
        _periodRepository = periodRepository;
    }

    public async Task<List<PeriodDto>> GetAllPeriods()
    {
        var periods = await _periodRepository.GetAllPeriods();
        return periods.Select(PeriodMapper.ToDto).ToList();
    }

    public async Task<PeriodDto?> GetPeriodById(int id)
    {
        var period = await _periodRepository.GetPeriodById(id);
        return period != null ? PeriodMapper.ToDto(period) : null;
    }

    public async Task<PeriodDto> CreatePeriod(CreatePeriodRequest request)
    {
        var period = PeriodMapper.ToEntity(request);
        var createdPeriod = await _periodRepository.CreatePeriod(period);
        return PeriodMapper.ToDto(createdPeriod);
    }

    public async Task<PeriodDto?> UpdatePeriod(int id, CreatePeriodRequest request)
    {
        var existingPeriod = await _periodRepository.GetPeriodById(id);
        if (existingPeriod == null)
            return null;

        PeriodMapper.UpdateEntity(existingPeriod, request);
        var updatedPeriod = await _periodRepository.UpdatePeriod(existingPeriod);
        return updatedPeriod != null ? PeriodMapper.ToDto(updatedPeriod) : null;
    }

    public async Task<bool> DeletePeriod(int id)
    {
        return await _periodRepository.DeletePeriod(id);
    }
}
