using DocTask.Core.Dtos.Frequency;
using DocTask.Core.Dtos.Period;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Service.Mappers;

namespace DocTask.Service.Services;

public class FrequencyService : IFrequencyService
{
    private readonly IFrequencyRepository _frequencyRepository;
    private readonly IPeriodRepository _periodRepository;

    public FrequencyService(IFrequencyRepository frequencyRepository, IPeriodRepository periodRepository)
    {
        _frequencyRepository = frequencyRepository;
        _periodRepository = periodRepository;
    }

    public async Task<List<FrequencyDto>> GetAllFrequencies()
    {
        var frequencies = await _frequencyRepository.GetAllFrequencies();
        return frequencies.Select(FrequencyMapper.ToDto).ToList();
    }

    public async Task<PaginationResponse<FrequencyDto>> GetFrequenciesPaginated(PaginationRequest request)
    {
        var (items, totalCount) = await _frequencyRepository.GetFrequenciesPaginated(request);
        var frequencyDtos = items.Select(FrequencyMapper.ToDto).ToList();
        
        return new PaginationResponse<FrequencyDto>
        {
            Data = frequencyDtos,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public async Task<FrequencyDto?> GetFrequencyById(int id)
    {
        var frequency = await _frequencyRepository.GetFrequencyById(id);
        return frequency != null ? FrequencyMapper.ToDto(frequency) : null;
    }

    public async Task<FrequencyDto> CreateFrequency(CreateFrequencyRequest request)
    {
        var frequency = FrequencyMapper.ToEntity(request);
        var createdFrequency = await _frequencyRepository.CreateFrequency(frequency);
        return FrequencyMapper.ToDto(createdFrequency);
    }

    public async Task<FrequencyDto?> UpdateFrequency(int id, CreateFrequencyRequest request)
    {
        var existingFrequency = await _frequencyRepository.GetFrequencyById(id);
        if (existingFrequency == null)
            return null;

        FrequencyMapper.UpdateEntity(existingFrequency, request);
        var updatedFrequency = await _frequencyRepository.UpdateFrequency(existingFrequency);
        return updatedFrequency != null ? FrequencyMapper.ToDto(updatedFrequency) : null;
    }

    public async Task<bool> DeleteFrequency(int id)
    {
        return await _frequencyRepository.DeleteFrequency(id);
    }

    public async Task<List<FrequencyDetailDto>> GetFrequencyDetailsByFrequencyId(int frequencyId)
    {
        var frequencyDetails = await _frequencyRepository.GetFrequencyDetailsByFrequencyId(frequencyId);
        return frequencyDetails.Select(FrequencyMapper.ToDto).ToList();
    }

    public async Task<PaginationResponse<FrequencyDetailDto>> GetFrequencyDetailsPaginated(int frequencyId, PaginationRequest request)
    {
        var (items, totalCount) = await _frequencyRepository.GetFrequencyDetailsPaginated(frequencyId, request);
        var frequencyDetailDtos = items.Select(FrequencyMapper.ToDto).ToList();
        
        return new PaginationResponse<FrequencyDetailDto>
        {
            Data = frequencyDetailDtos,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public async Task<FrequencyDetailDto> CreateFrequencyDetail(CreateFrequencyDetailRequest request)
    {
        var frequencyDetail = FrequencyMapper.ToEntity(request);
        var createdFrequencyDetail = await _frequencyRepository.CreateFrequencyDetail(frequencyDetail);
        return FrequencyMapper.ToDto(createdFrequencyDetail);
    }

    public async Task<bool> DeleteFrequencyDetail(int id)
    {
        return await _frequencyRepository.DeleteFrequencyDetail(id);
    }

    public async Task<CreateFrequencyWithPeriodsResponse> CreateFrequencyWithPeriods(CreateFrequencyWithPeriodsRequest request)
    {
        // Create the frequency first
        var frequencyRequest = new CreateFrequencyRequest
        {
            FrequencyType = request.FrequencyType,
            FrequencyDetail = request.FrequencyDetail,
            IntervalValue = request.IntervalValue
        };
        
        var frequency = await CreateFrequency(frequencyRequest);
        
        // Generate periods based on frequency type and date range
        var periods = GeneratePeriodsBasedOnFrequency(request.FrequencyType, request.StartDate, request.EndDate, request.IntervalValue);
        
        // Create periods in database
        var createdPeriods = new List<PeriodDto>();
        foreach (var period in periods)
        {
            var periodRequest = new CreatePeriodRequest
            {
                PeriodName = period.PeriodName,
                StartDate = period.StartDate,
                EndDate = period.EndDate
            };
            
            var createdPeriod = await _periodRepository.CreatePeriod(PeriodMapper.ToEntity(periodRequest));
            createdPeriods.Add(PeriodMapper.ToDto(createdPeriod));
        }
        
        return new CreateFrequencyWithPeriodsResponse
        {
            Frequency = frequency,
            CreatedPeriods = createdPeriods,
            TotalPeriodsCreated = createdPeriods.Count
        };
    }
    
    private List<PeriodDto> GeneratePeriodsBasedOnFrequency(string frequencyType, DateTime startDate, DateTime endDate, int intervalValue, List<int>? daysOfWeek = null, int? dayOfMonth = null)
    {
        var periods = new List<PeriodDto>();
        var currentDate = startDate;
        var periodCounter = 1;
        
        while (currentDate <= endDate)
        {
            DateTime periodEndDate = currentDate;
            string periodName = string.Empty;
            
            switch (frequencyType.ToLower())
            {
                case "daily":
                    periodEndDate = currentDate.AddDays(1).AddSeconds(-1);
                    periodName = $"Day {periodCounter} - {currentDate:MMM dd, yyyy}";
                    currentDate = currentDate.AddDays(intervalValue);
                    break;
                    
                case "weekly":
                    // If daysOfWeek provided, create one period per chosen day
                    if (daysOfWeek != null && daysOfWeek.Any())
                    {
                        foreach (var dow in daysOfWeek.OrderBy(d => d))
                        {
                            var next = NextDayOfWeek(currentDate, (DayOfWeek)dow);
                            if (next > endDate) break;
                            var pEnd = next.AddDays(1).AddSeconds(-1);
                            periods.Add(new PeriodDto
                            {
                                PeriodId = 0,
                                PeriodName = $"Week {periodCounter} - {next:dddd, MMM dd}",
                                StartDate = next.Date,
                                EndDate = pEnd,
                                CreatedAt = DateTime.UtcNow
                            });
                            periodCounter++;
                        }
                        currentDate = currentDate.AddDays(7 * intervalValue);
                        continue;
                    }
                    periodEndDate = currentDate.AddDays(7).AddSeconds(-1);
                    periodName = $"Week {periodCounter} - {currentDate:MMM dd} to {periodEndDate:MMM dd, yyyy}";
                    currentDate = currentDate.AddDays(7 * intervalValue);
                    break;
                    
                case "monthly":
                    if (dayOfMonth.HasValue)
                    {
                        var nextMonthDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(0);
                        var targetDay = Math.Min(dayOfMonth.Value, DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month));
                        var occurrence = new DateTime(nextMonthDate.Year, nextMonthDate.Month, targetDay);
                        if (occurrence < currentDate) occurrence = occurrence.AddMonths(intervalValue);
                        if (occurrence > endDate) break;
                        periods.Add(new PeriodDto
                        {
                            PeriodId = 0,
                            PeriodName = $"Month {periodCounter} - {occurrence:MMM dd, yyyy}",
                            StartDate = occurrence,
                            EndDate = occurrence.AddDays(1).AddSeconds(-1),
                            CreatedAt = DateTime.UtcNow
                        });
                        periodCounter++;
                        currentDate = occurrence.AddMonths(intervalValue);
                        continue;
                    }
                    periodEndDate = currentDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    periodName = $"Month {periodCounter} - {currentDate:MMMM yyyy}";
                    currentDate = currentDate.AddMonths(intervalValue);
                    break;
                    
                case "quarterly":
                    periodEndDate = currentDate.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    periodName = $"Q{GetQuarter(currentDate)} {currentDate.Year} - {currentDate:MMM} to {periodEndDate:MMM}";
                    currentDate = currentDate.AddMonths(3 * intervalValue);
                    break;
                    
                case "yearly":
                    periodEndDate = currentDate.AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    periodName = $"Year {periodCounter} - {currentDate.Year}";
                    currentDate = currentDate.AddYears(intervalValue);
                    break;
                    
                default:
                    // Default to daily if frequency type is not recognized
                    periodEndDate = currentDate.AddDays(1).AddSeconds(-1);
                    periodName = $"Period {periodCounter} - {currentDate:MMM dd, yyyy}";
                    currentDate = currentDate.AddDays(intervalValue);
                    break;
            }
            
            // Ensure we don't exceed the end date
            if (periodEndDate > endDate)
            {
                periodEndDate = endDate;
            }
            
            periods.Add(new PeriodDto
            {
                PeriodId = 0, // Will be set by database
                PeriodName = periodName,
                StartDate = currentDate.AddDays(-GetDaysToSubtract(frequencyType, intervalValue)),
                EndDate = periodEndDate,
                CreatedAt = DateTime.UtcNow
            });
            
            periodCounter++;
            
            // Break if we've reached or exceeded the end date
            if (currentDate > endDate)
                break;
        }
        
        return periods;
    }
    
    private int GetQuarter(DateTime date)
    {
        return (date.Month - 1) / 3 + 1;
    }
    
    private int GetDaysToSubtract(string frequencyType, int intervalValue)
    {
        return frequencyType.ToLower() switch
        {
            "daily" => intervalValue,
            "weekly" => 7 * intervalValue,
            "monthly" => 0, // Will be handled by AddMonths
            "quarterly" => 0, // Will be handled by AddMonths
            "yearly" => 0, // Will be handled by AddYears
            _ => intervalValue
        };
    }

    private static DateTime NextDayOfWeek(DateTime start, DayOfWeek day)
    {
        int diff = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.Date.AddDays(diff);
    }
}
