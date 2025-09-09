using DocTask.Core.Dtos.Frequency;
using DocTask.Core.Models;

namespace DocTask.Service.Mappers;

public static class FrequencyMapper
{
    public static FrequencyDto ToDto(Frequency frequency)
    {
        return new FrequencyDto
        {
            FrequencyId = frequency.FrequencyId,
            FrequencyType = frequency.FrequencyType,
            FrequencyDetail = frequency.FrequencyDetail,
            IntervalValue = frequency.IntervalValue,
            CreatedAt = frequency.CreatedAt
        };
    }

    public static Frequency ToEntity(CreateFrequencyRequest request)
    {
        return new Frequency
        {
            FrequencyType = request.FrequencyType,
            FrequencyDetail = request.FrequencyDetail,
            IntervalValue = request.IntervalValue,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateEntity(Frequency existingFrequency, CreateFrequencyRequest request)
    {
        existingFrequency.FrequencyType = request.FrequencyType;
        existingFrequency.FrequencyDetail = request.FrequencyDetail;
        existingFrequency.IntervalValue = request.IntervalValue;
    }

    public static FrequencyDetailDto ToDto(FrequencyDetail frequencyDetail)
    {
        return new FrequencyDetailDto
        {
            Id = frequencyDetail.Id,
            FrequencyId = frequencyDetail.FrequencyId,
            DayOfWeek = frequencyDetail.DayOfWeek,
            DayOfMonth = frequencyDetail.DayOfMonth
        };
    }

    public static FrequencyDetail ToEntity(CreateFrequencyDetailRequest request)
    {
        return new FrequencyDetail
        {
            FrequencyId = request.FrequencyId,
            DayOfWeek = request.DayOfWeek,
            DayOfMonth = request.DayOfMonth
        };
    }
}
