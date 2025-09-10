using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Frequency;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/frequency")]
[Authorize]
public class FrequencyController : ControllerBase
{
    private readonly IFrequencyService _frequencyService;

    public FrequencyController(IFrequencyService frequencyService)
    {
        _frequencyService = frequencyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFrequencies([FromQuery] PaginationRequest? request = null)
    {
        if (request != null)
        {
            var frequencies = await _frequencyService.GetFrequenciesPaginated(request);
            return Ok(new PaginatedApiResponse<FrequencyDto>(frequencies, "Frequencies retrieved successfully"));
        }
        else
        {
            var frequencies = await _frequencyService.GetAllFrequencies();
            return Ok(new ApiResponse<List<FrequencyDto>> { Success = true, Data = frequencies });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFrequencyById(int id)
    {
        var frequency = await _frequencyService.GetFrequencyById(id);
        if (frequency == null)
        {
            return NotFound(new ApiResponse<FrequencyDto> { Success = false, Error = "Frequency not found" });
        }
        return Ok(new ApiResponse<FrequencyDto> { Success = true, Data = frequency });
    }

    [HttpPost]
    public async Task<IActionResult> CreateFrequency([FromBody] CreateFrequencyRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<FrequencyDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var frequency = await _frequencyService.CreateFrequency(request);
            return CreatedAtAction(nameof(GetFrequencyById), new { id = frequency.FrequencyId }, 
                new ApiResponse<FrequencyDto> { Success = true, Data = frequency });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<FrequencyDto> { Success = false, Error = $"Error creating frequency: {ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFrequency(int id, [FromBody] CreateFrequencyRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<FrequencyDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var frequency = await _frequencyService.UpdateFrequency(id, request);
            if (frequency == null)
            {
                return NotFound(new ApiResponse<FrequencyDto> { Success = false, Error = "Frequency not found" });
            }
            return Ok(new ApiResponse<FrequencyDto> { Success = true, Data = frequency });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<FrequencyDto> { Success = false, Error = $"Error updating frequency: {ex.Message}" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFrequency(int id)
    {
        try
        {
            var result = await _frequencyService.DeleteFrequency(id);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Error = "Frequency not found" });
            }
            return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Frequency deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool> { Success = false, Error = $"Error deleting frequency: {ex.Message}" });
        }
    }

    [HttpGet("{frequencyId}/details")]
    public async Task<IActionResult> GetFrequencyDetails(int frequencyId, [FromQuery] PaginationRequest? request = null)
    {
        if (request != null)
        {
            var details = await _frequencyService.GetFrequencyDetailsPaginated(frequencyId, request);
            return Ok(new PaginatedApiResponse<FrequencyDetailDto>(details, "Frequency details retrieved successfully"));
        }
        else
        {
            var details = await _frequencyService.GetFrequencyDetailsByFrequencyId(frequencyId);
            return Ok(new ApiResponse<List<FrequencyDetailDto>> { Success = true, Data = details });
        }
    }

    [HttpPost("details")]
    public async Task<IActionResult> CreateFrequencyDetail([FromBody] CreateFrequencyDetailRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<FrequencyDetailDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var detail = await _frequencyService.CreateFrequencyDetail(request);
            return Ok(new ApiResponse<FrequencyDetailDto> { Success = true, Data = detail });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<FrequencyDetailDto> { Success = false, Error = $"Error creating frequency detail: {ex.Message}" });
        }
    }

    [HttpDelete("details/{id}")]
    public async Task<IActionResult> DeleteFrequencyDetail(int id)
    {
        try
        {
            var result = await _frequencyService.DeleteFrequencyDetail(id);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Error = "Frequency detail not found" });
            }
            return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Frequency detail deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool> { Success = false, Error = $"Error deleting frequency detail: {ex.Message}" });
        }
    }

    [HttpPost("with-periods")]
    public async Task<IActionResult> CreateFrequencyWithPeriods([FromBody] CreateFrequencyWithPeriodsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<CreateFrequencyWithPeriodsResponse> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var result = await _frequencyService.CreateFrequencyWithPeriods(request);
            return Ok(new ApiResponse<CreateFrequencyWithPeriodsResponse> 
            { 
                Success = true, 
                Data = result,
                Message = $"Successfully created frequency and {result.TotalPeriodsCreated} periods"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<CreateFrequencyWithPeriodsResponse> 
            { 
                Success = false, 
                Error = $"Error creating frequency with periods: {ex.Message}" 
            });
        }
    }
}
