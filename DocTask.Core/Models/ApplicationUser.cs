using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("AspNetUsers")]
[Index("Email", Name = "email", IsUnique = true)]
[Index("UnitUserId", Name = "fk_user_unitUser")]
[Index("OrgId", Name = "orgId")]
[Index("UnitId", Name = "unitId")]
[Index("UserParent", Name = "userParent")]
[Index("PositionId", Name = "user_ibfk_1")]
[Index("UserName", Name = "username", IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    [Column("fullName")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("phoneNumber")]
    [StringLength(20)]
    public new string? PhoneNumber { get; set; }

    [Column("orgId")]
    public int? OrgId { get; set; }

    [Column("unitId")]
    public int? UnitId { get; set; }

    [Column("userParent")]
    public string? UserParent { get; set; }

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

    // Navigation properties
    [InverseProperty("UserParentNavigation")]
    public virtual ICollection<ApplicationUser> InverseUserParentNavigation { get; set; } = new List<ApplicationUser>();

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
    public virtual ApplicationUser? UserParentNavigation { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
