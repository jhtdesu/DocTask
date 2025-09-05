using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("progress")]
[Index("PeriodId", Name = "fkProgressPeriod")]
[Index("TaskId", Name = "fkProgressTask")]
[Index("UpdatedBy", Name = "fkProgressUpdatedBy")]
public partial class Progress
{
    [Key]
    [Column("progressId")]
    public int ProgressId { get; set; }

    [Column("taskId")]
    public int TaskId { get; set; }

    [Column("periodId")]
    public int? PeriodId { get; set; }

    [Column("percentageComplete")]
    public int? PercentageComplete { get; set; }

    [Column("comment", TypeName = "text")]
    public string? Comment { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [Column("fileName")]
    [StringLength(255)]
    public string? FileName { get; set; }

    [Column("filePath")]
    [StringLength(255)]
    public string? FilePath { get; set; }

    [Column("proposal", TypeName = "text")]
    public string? Proposal { get; set; }

    [Column("result", TypeName = "text")]
    public string? Result { get; set; }

    [Column("feedback", TypeName = "text")]
    public string? Feedback { get; set; }

    [ForeignKey("PeriodId")]
    [InverseProperty("Progresses")]
    public virtual Period? Period { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("Progresses")]
    public virtual Task Task { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("Progresses")]
    public virtual ApplicationUser? UpdatedByNavigation { get; set; }
}
