namespace DocTask.Core.Dtos.Period;

public class PeriodDto
{
    public int PeriodId { get; set; }
    public string PeriodName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePeriodRequest
{
    public string PeriodName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
