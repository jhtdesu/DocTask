using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("notification")]
[Index("TaskId", Name = "fkNotificationTask")]
[Index("UserId", Name = "fkNotificationUser")]
public partial class Notification
{
    [Key]
    [Column("notificationId")]
    public int NotificationId { get; set; }

    [Column("userId")]
    public string? UserId { get; set; }

    [Column("taskId")]
    public int? TaskId { get; set; }

    [Column("message", TypeName = "text")]
    public string Message { get; set; } = null!;

    [Column("isRead")]
    public bool? IsRead { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Notification")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    [ForeignKey("TaskId")]
    [InverseProperty("Notifications")]
    public virtual Task? Task { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual ApplicationUser? User { get; set; }
}
