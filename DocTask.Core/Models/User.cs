using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("user")]
[Index("Email", Name = "email", IsUnique = true)]
[Index("UnitUserId", Name = "fk_user_unitUser")]
[Index("OrgId", Name = "orgId")]
[Index("UnitId", Name = "unitId")]
[Index("UserParent", Name = "userParent")]
[Index("PositionId", Name = "user_ibfk_1")]
[Index("Username", Name = "username", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("userId")]
    public int UserId { get; set; }

    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("fullName")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("phoneNumber")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("orgId")]
    public int? OrgId { get; set; }

    [Column("unitId")]
    public int? UnitId { get; set; }

    [Column("userParent")]
    public int? UserParent { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("refreshtoken")]
    [StringLength(255)]
    public string? Refreshtoken { get; set; }

    [Column("refreshtokenexpirytime")]
    public DateTime? Refreshtokenexpirytime { get; set; }

    [Column("role")]
    [StringLength(11)]
    public string Role { get; set; } = null!;

    [Column("unitUserId")]
    public int? UnitUserId { get; set; }

    [Column("positionId")]
    public int? PositionId { get; set; }

    [Column("positionName")]
    [StringLength(255)]
    public string? PositionName { get; set; }

    [InverseProperty("UserParentNavigation")]
    public virtual ICollection<User> InverseUserParentNavigation { get; set; } = new List<User>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("OrgId")]
    [InverseProperty("Users")]
    public virtual Org? Org { get; set; }

    [ForeignKey("PositionId")]
    [InverseProperty("Users")]
    public virtual Position? Position { get; set; }

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    [InverseProperty("Assignee")]
    public virtual ICollection<Task> TaskAssignees { get; set; } = new List<Task>();

    [InverseProperty("Assigner")]
    public virtual ICollection<Task> TaskAssigners { get; set; } = new List<Task>();

    [ForeignKey("UnitId")]
    [InverseProperty("Users")]
    public virtual Unit? Unit { get; set; }

    [ForeignKey("UnitUserId")]
    [InverseProperty("Users")]
    public virtual Unituser? UnitUser { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Unituser> Unitusers { get; set; } = new List<Unituser>();

    [InverseProperty("UploadedByNavigation")]
    public virtual ICollection<Uploadfile> Uploadfiles { get; set; } = new List<Uploadfile>();

    [ForeignKey("UserParent")]
    [InverseProperty("InverseUserParentNavigation")]
    public virtual User? UserParentNavigation { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
