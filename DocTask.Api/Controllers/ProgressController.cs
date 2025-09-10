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
    public async Task<IActionResult> GetAllProgress([FromQuery] DateTime? startDate, [FromQuery] DateTime? dueDate, [FromQuery] PaginationRequest? request = null)
    {
        try
        {
            if (request != null)
            {
                var progress = await _progressService.GetProgressPaginated(request, startDate, dueDate);
                return Ok(new PaginatedApiResponse<ProgressDto>(progress, "Progress retrieved successfully"));
            }
            else
            {
                var progress = await _progressService.GetAllProgress(startDate, dueDate);
                return Ok(new ApiResponse<List<ProgressDto>> { Success = true, Data = progress });
            }
        }
        catch (Exception ex)
        {
            if (request != null)
            {
                return StatusCode(500, new PaginatedApiResponse<ProgressDto>(false, $"Error retrieving progress: {ex.Message}"));
            }
            else
            {
                return StatusCode(500, new ApiResponse<List<ProgressDto>> { Success = false, Error = $"Error retrieving progress: {ex.Message}" });
            }
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProgress([FromForm] CreateProgressDto createProgressDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProgressDto> { Success = false, Error = "Invalid model state" });
            }

            // Get the current user ID from claims
            var userId = User.FindFirst("sub")?.Value ?? 
                        User.FindFirst("nameid")?.Value ?? 
                        User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ??
                        User.FindFirst("id")?.Value ??
                        User.Identity?.Name;
            
            if (string.IsNullOrEmpty(userId))
            {
                // Debug information - return available claims
                var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Unauthorized(new ApiResponse<ProgressDto> { 
                    Success = false, 
                    Error = $"User not authenticated. Available claims: {string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}"))}" 
                });
            }

            var progress = await _progressService.CreateProgress(createProgressDto, userId);
            return CreatedAtAction(nameof(GetProgressById), new { id = progress.ProgressId }, 
                new ApiResponse<ProgressDto> { Success = true, Data = progress, Message = "Progress created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProgressDto> { Success = false, Error = $"Error creating progress: {ex.Message}" });
        }
    }

}
