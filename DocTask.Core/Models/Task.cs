using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("task")]
[Index("AssigneeId", Name = "fkTaskAssignee")]
[Index("AssignerId", Name = "fkTaskAssigner")]
[Index("AttachedFile", Name = "fkTaskAttachedFile")]
[Index("OrgId", Name = "fkTaskOrg")]
[Index("PeriodId", Name = "fkTaskPeriod")]
[Index("FrequencyId", Name = "fk_taskitem_frequency")]
public partial class Task
{
    [Key]
    [Column("taskId")]
    public int TaskId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("assignerId")]
    public string? AssignerId { get; set; }

    [Column("assigneeId")]
    public string? AssigneeId { get; set; }

    [Column("orgId")]
    public int? OrgId { get; set; }

    [Column("periodId")]
    public int? PeriodId { get; set; }

    [Column("attachedFile")]
    public int? AttachedFile { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("priority")]
    [StringLength(50)]
    public string? Priority { get; set; }

    [Column("startDate")]
    public DateTime? StartDate { get; set; }

    [Column("dueDate")]
    public DateTime? DueDate { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("unitId")]
    public int? UnitId { get; set; }

    [Column("frequencyId")]
    public int? FrequencyId { get; set; }

    [Column("percentagecomplete")]
    public int? Percentagecomplete { get; set; }

    [Column("parentTaskId")]
    public int? ParentTaskId { get; set; }

    [ForeignKey("AssigneeId")]
    [InverseProperty("TaskAssignees")]
    public virtual ApplicationUser? Assignee { get; set; }

    [ForeignKey("AssignerId")]
    [InverseProperty("TaskAssigners")]
    public virtual ApplicationUser? Assigner { get; set; }

    [ForeignKey("AttachedFile")]
    [InverseProperty("Tasks")]
    public virtual Uploadfile? AttachedFileNavigation { get; set; }

    [ForeignKey("FrequencyId")]
    [InverseProperty("Tasks")]
    public virtual Frequency? Frequency { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("OrgId")]
    [InverseProperty("Tasks")]
    public virtual Org? Org { get; set; }

    [ForeignKey("PeriodId")]
    [InverseProperty("Tasks")]
    public virtual Period? Period { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    [InverseProperty("Task")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    [InverseProperty("Task")]
    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    [InverseProperty("Task")]
    public virtual ICollection<Taskunitassignment> Taskunitassignments { get; set; } = new List<Taskunitassignment>();

    [ForeignKey("TaskId")]
    [InverseProperty("Tasks")]
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
