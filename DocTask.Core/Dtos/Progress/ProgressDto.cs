namespace DocTask.Core.Dtos.Progress;

public class ProgressDto
{
    public int ProgressId { get; set; }
    public int TaskId { get; set; }
    public int? PeriodId { get; set; }
    public int? PercentageComplete { get; set; }
    public string? Comment { get; set; }
    public string? Status { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public string? Proposal { get; set; }
    public string? Result { get; set; }
    public string? Feedback { get; set; }
    
    // Navigation properties
    public string? PeriodName { get; set; }
    public string? TaskName { get; set; }
    public string? UpdatedByUserName { get; set; }
}
