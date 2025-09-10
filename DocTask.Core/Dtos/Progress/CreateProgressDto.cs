using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DocTask.Core.Dtos.Progress;

public class CreateProgressDto
{
    [Required]
    public int TaskId { get; set; }
    
    [Required]
    public int PeriodId { get; set; }
    
    public string? Proposal { get; set; }
    
    public string? Result { get; set; }
    
    public string? Feedback { get; set; }
    
    public IFormFile? ReportFile { get; set; }
    
    public int? PercentageComplete { get; set; }
    
    public string? Comment { get; set; }
    
    public string? Status { get; set; }
}
