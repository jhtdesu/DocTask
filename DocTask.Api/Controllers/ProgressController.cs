using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Progress;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/progress")]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProgress([FromQuery] DateTime? startDate, [FromQuery] DateTime? dueDate)
    {
        try
        {
            var progress = await _progressService.GetAllProgress(startDate, dueDate);
            return Ok(new ApiResponse<List<ProgressDto>> { Success = true, Data = progress });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<ProgressDto>> { Success = false, Error = $"Error retrieving progress: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgressById(int id)
    {
        try
        {
            var progress = await _progressService.GetProgressById(id);
            if (progress == null)
            {
                return NotFound(new ApiResponse<ProgressDto> { Success = false, Error = "Progress not found" });
            }
            return Ok(new ApiResponse<ProgressDto> { Success = true, Data = progress });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProgressDto> { Success = false, Error = $"Error retrieving progress: {ex.Message}" });
        }
    }

}
