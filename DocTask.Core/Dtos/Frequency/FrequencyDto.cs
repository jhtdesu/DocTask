using DocTask.Core.Dtos.Period;

namespace DocTask.Core.Dtos.Frequency;

public class FrequencyDto
{
    public int FrequencyId { get; set; }
    public string FrequencyType { get; set; } = null!;
    public string? FrequencyDetail { get; set; }
    public int IntervalValue { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFrequencyRequest
{
    public string FrequencyType { get; set; } = null!;
    public string? FrequencyDetail { get; set; }
    public int IntervalValue { get; set; }
}

public class FrequencyDetailDto
{
    public int Id { get; set; }
    public int FrequencyId { get; set; }
    public sbyte? DayOfWeek { get; set; }
    public sbyte? DayOfMonth { get; set; }
}

public class CreateFrequencyDetailRequest
{
    public int FrequencyId { get; set; }
    public sbyte? DayOfWeek { get; set; }
    public sbyte? DayOfMonth { get; set; }
}

public class CreateFrequencyWithPeriodsRequest
{
    public string FrequencyType { get; set; } = null!;
    public string? FrequencyDetail { get; set; }
    public int IntervalValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    // Optional helpers to drive period generation
    public List<int>? DaysOfWeek { get; set; } // 0=Sunday..6=Saturday
    public int? DayOfMonth { get; set; } // 1..31
}

// Unified request to support creating just a frequency or frequency with periods
public class CreateFrequencyUnifiedRequest
{
    public string FrequencyType { get; set; } = null!;
    public string? FrequencyDetail { get; set; }
    public int IntervalValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class CreateFrequencyWithPeriodsResponse
{
    public FrequencyDto Frequency { get; set; } = null!;
    public List<PeriodDto> CreatedPeriods { get; set; } = new List<PeriodDto>();
    public int TotalPeriodsCreated { get; set; }
}
