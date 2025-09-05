using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTask.Core.Models;

[Table("period")]
public partial class Period
{
    [Key]
    [Column("periodId")]
    public int PeriodId { get; set; }

    [Column("periodName")]
    [StringLength(50)]
    public string PeriodName { get; set; } = null!;

    [Column("startDate", TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column("endDate", TypeName = "date")]
    public DateTime EndDate { get; set; }

    [Column("createdAt", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Period")]
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    [InverseProperty("Period")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    [InverseProperty("Period")]
    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    [InverseProperty("Period")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
