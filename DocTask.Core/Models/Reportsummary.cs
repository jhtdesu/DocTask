using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("reportsummary")]
[Index("CreatedBy", Name = "fkReportCreatedBy")]
[Index("ReportFile", Name = "fkReportFile")]
[Index("PeriodId", Name = "fkReportPeriod")]
[Index("TaskId", Name = "fkReportTask")]
public partial class Reportsummary
{
    [Key]
    [Column("reportId")]
    public int ReportId { get; set; }

    [Column("taskId")]
    public int? TaskId { get; set; }

    [Column("periodId")]
    public int? PeriodId { get; set; }

    [Column("summary", TypeName = "text")]
    public string? Summary { get; set; }

    [Column("createdBy")]
    public int? CreatedBy { get; set; }

    [Column("reportFile")]
    public int? ReportFile { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Reportsummaries")]
    public virtual User? CreatedByNavigation { get; set; }

    [ForeignKey("PeriodId")]
    [InverseProperty("Reportsummaries")]
    public virtual Period? Period { get; set; }

    [ForeignKey("ReportFile")]
    [InverseProperty("Reportsummaries")]
    public virtual Uploadfile? ReportFileNavigation { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("Reportsummaries")]
    public virtual Task? Task { get; set; }
}
