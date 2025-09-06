using DocTask.Core.Dtos.Progress;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Service.Mappers;

namespace DocTask.Service.Services;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;

    public ProgressService(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }

    public async Task<List<ProgressDto>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null)
    {
        var progress = await _progressRepository.GetAllProgress(startDate, dueDate);
        return ProgressMapper.ToDtoList(progress);
    }

    public async Task<ProgressDto?> GetProgressById(int id)
    {
        var progress = await _progressRepository.GetProgressById(id);
        return progress != null ? ProgressMapper.ToDto(progress) : null;
    }
}
