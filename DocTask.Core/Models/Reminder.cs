using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("reminder")]
[Index("Createdby", Name = "createdby")]
[Index("Notificationid", Name = "notificationid")]
[Index("Periodid", Name = "periodid")]
[Index("Taskid", Name = "taskid")]
public partial class Reminder
{
    [Key]
    [Column("reminderid")]
    public int Reminderid { get; set; }

    [Column("taskid")]
    public int Taskid { get; set; }

    [Column("periodid")]
    public int? Periodid { get; set; }

    [Column("message", TypeName = "text")]
    public string Message { get; set; } = null!;

    [Column("triggertime")]
    public DateTime Triggertime { get; set; }

    [Column("isauto")]
    public bool? Isauto { get; set; }

    [Column("createdby")]
    public string? Createdby { get; set; }

    [Column("createdat")]
    public DateTime Createdat { get; set; }

    [Column("isnotified")]
    public bool? Isnotified { get; set; }

    [Column("notifiedat")]
    public DateTime? Notifiedat { get; set; }

    [Column("notificationid")]
    public int? Notificationid { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("Reminders")]
    public virtual ApplicationUser? CreatedbyNavigation { get; set; }

    [ForeignKey("Notificationid")]
    [InverseProperty("Reminders")]
    public virtual Notification? Notification { get; set; }

    [ForeignKey("Periodid")]
    [InverseProperty("Reminders")]
    public virtual Period? Period { get; set; }

    [InverseProperty("Reminder")]
    public virtual ICollection<Reminderunit> Reminderunits { get; set; } = new List<Reminderunit>();

    [ForeignKey("Taskid")]
    [InverseProperty("Reminders")]
    public virtual Task Task { get; set; } = null!;
}
