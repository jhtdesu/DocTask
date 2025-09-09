using DocTask.Core.Dtos.Period;
using DocTask.Core.Models;

namespace DocTask.Service.Mappers;

public static class PeriodMapper
{
    public static PeriodDto ToDto(Period period)
    {
        return new PeriodDto
        {
            PeriodId = period.PeriodId,
            PeriodName = period.PeriodName,
            StartDate = period.StartDate,
            EndDate = period.EndDate,
            CreatedAt = period.CreatedAt
        };
    }

    public static Period ToEntity(CreatePeriodRequest request)
    {
        return new Period
        {
            PeriodName = request.PeriodName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateEntity(Period existingPeriod, CreatePeriodRequest request)
    {
        existingPeriod.PeriodName = request.PeriodName;
        existingPeriod.StartDate = request.StartDate;
        existingPeriod.EndDate = request.EndDate;
    }
}
