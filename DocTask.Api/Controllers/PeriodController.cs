using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Period;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/period")]
[Authorize]
public class PeriodController : ControllerBase
{
    private readonly IPeriodService _periodService;

    public PeriodController(IPeriodService periodService)
    {
        _periodService = periodService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPeriods([FromQuery] PaginationRequest? request = null)
    {
        if (request != null)
        {
            var periods = await _periodService.GetPeriodsPaginated(request);
            return Ok(new PaginatedApiResponse<PeriodDto>(periods, "Periods retrieved successfully"));
        }
        else
        {
            var periods = await _periodService.GetAllPeriods();
            return Ok(new ApiResponse<List<PeriodDto>> { Success = true, Data = periods });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPeriodById(int id)
    {
        var period = await _periodService.GetPeriodById(id);
        if (period == null)
        {
            return NotFound(new ApiResponse<PeriodDto> { Success = false, Error = "Period not found" });
        }
        return Ok(new ApiResponse<PeriodDto> { Success = true, Data = period });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePeriod([FromBody] CreatePeriodRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<PeriodDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var period = await _periodService.CreatePeriod(request);
            return CreatedAtAction(nameof(GetPeriodById), new { id = period.PeriodId }, 
                new ApiResponse<PeriodDto> { Success = true, Data = period });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PeriodDto> { Success = false, Error = $"Error creating period: {ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePeriod(int id, [FromBody] CreatePeriodRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<PeriodDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var period = await _periodService.UpdatePeriod(id, request);
            if (period == null)
            {
                return NotFound(new ApiResponse<PeriodDto> { Success = false, Error = "Period not found" });
            }
            return Ok(new ApiResponse<PeriodDto> { Success = true, Data = period });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PeriodDto> { Success = false, Error = $"Error updating period: {ex.Message}" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePeriod(int id)
    {
        try
        {
            var result = await _periodService.DeletePeriod(id);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Error = "Period not found" });
            }
            return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Period deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool> { Success = false, Error = $"Error deleting period: {ex.Message}" });
        }
    }
}
