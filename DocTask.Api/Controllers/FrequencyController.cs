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
    public async Task<IActionResult> CreateFrequency([FromBody] CreateFrequencyUnifiedRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object> { Success = false, Error = "Invalid request data" });
        }
        // If dates are provided, create frequency and periods together
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            var comboReq = new CreateFrequencyWithPeriodsRequest
            {
                FrequencyType = request.FrequencyType,
                FrequencyDetail = request.FrequencyDetail,
                IntervalValue = request.IntervalValue,
                StartDate = request.StartDate.Value,
                EndDate = request.EndDate.Value
            };

            var result = await _frequencyService.CreateFrequencyWithPeriods(comboReq);
            return Ok(new ApiResponse<CreateFrequencyWithPeriodsResponse>
            {
                Success = true,
                Data = result,
                Message = $"Successfully created frequency and {result.TotalPeriodsCreated} periods"
            });
        }

        // Otherwise, create just the frequency
        var createOnly = new CreateFrequencyRequest
        {
            FrequencyType = request.FrequencyType,
            FrequencyDetail = request.FrequencyDetail,
            IntervalValue = request.IntervalValue
        };

        var frequency = await _frequencyService.CreateFrequency(createOnly);
        return CreatedAtAction(nameof(GetFrequencyById), new { id = frequency.FrequencyId },
            new ApiResponse<FrequencyDto> { Success = true, Data = frequency });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFrequency(int id, [FromBody] CreateFrequencyRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<FrequencyDto> { Success = false, Error = "Invalid request data" });
        }
        var frequency = await _frequencyService.UpdateFrequency(id, request);
        if (frequency == null)
        {
            return NotFound(new ApiResponse<FrequencyDto> { Success = false, Error = "Frequency not found" });
        }
        return Ok(new ApiResponse<FrequencyDto> { Success = true, Data = frequency });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFrequency(int id)
    {
        var result = await _frequencyService.DeleteFrequency(id);
        if (!result)
        {
            return NotFound(new ApiResponse<bool> { Success = false, Error = "Frequency not found" });
        }
        return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Frequency deleted successfully" });
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
        var detail = await _frequencyService.CreateFrequencyDetail(request);
        return Ok(new ApiResponse<FrequencyDetailDto> { Success = true, Data = detail });
    }

    [HttpDelete("details/{id}")]
    public async Task<IActionResult> DeleteFrequencyDetail(int id)
    {
        var result = await _frequencyService.DeleteFrequencyDetail(id);
        if (!result)
        {
            return NotFound(new ApiResponse<bool> { Success = false, Error = "Frequency detail not found" });
        }
        return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Frequency detail deleted successfully" });
    }

    // Removed excess: unified into POST /api/v1/frequency
}
