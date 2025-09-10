using DocTask.Core.Dtos.Progress;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Service.Mappers;
using DocTask.Core.Dtos.UploadFile;

namespace DocTask.Service.Services;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IUploadFileService _uploadFileService;

    public ProgressService(IProgressRepository progressRepository, IUploadFileService uploadFileService)
    {
        _progressRepository = progressRepository;
        _uploadFileService = uploadFileService;
    }

    public async Task<List<ProgressDto>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null)
    {
        var progress = await _progressRepository.GetAllProgress(startDate, dueDate);
        return ProgressMapper.ToDtoList(progress);
    }

    public async Task<PaginationResponse<ProgressDto>> GetProgressPaginated(PaginationRequest request, DateTime? startDate = null, DateTime? dueDate = null)
    {
        var (items, totalCount) = await _progressRepository.GetProgressPaginated(request, startDate, dueDate);
        var progressDtos = ProgressMapper.ToDtoList(items);
        
        return new PaginationResponse<ProgressDto>
        {
            Data = progressDtos,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public async Task<ProgressDto?> GetProgressById(int id)
    {
        var progress = await _progressRepository.GetProgressById(id);
        return progress != null ? ProgressMapper.ToDto(progress) : null;
    }

    public async Task<ProgressDto> CreateProgress(CreateProgressDto createProgressDto, string updatedBy)
    {
        var progress = ProgressMapper.ToModel(createProgressDto, updatedBy);
        
        // Handle file upload if ReportFile is provided
        if (createProgressDto.ReportFile != null && createProgressDto.ReportFile.Length > 0)
        {
            var uploadRequest = new UploadFileRequest
            {
                File = createProgressDto.ReportFile
            };
            
            var uploadedFile = await _uploadFileService.UploadFileAsync(uploadRequest, updatedBy);
            progress.FileName = uploadedFile.FileName;
            progress.FilePath = uploadedFile.FilePath;
        }
        
        var createdProgress = await _progressRepository.CreateProgress(progress);
        return ProgressMapper.ToDto(createdProgress);
    }
}
