using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("unit")]
[Index("OrgId", Name = "fkUnitOrg")]
[Index("UnitParent", Name = "fkUnitParent")]
public partial class Unit
{
    [Key]
    [Column("unitId")]
    public int UnitId { get; set; }

    [Column("orgId")]
    public int OrgId { get; set; }

    [Column("unitName")]
    [StringLength(255)]
    public string UnitName { get; set; } = null!;

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }

    [Column("unitParent")]
    public int? UnitParent { get; set; }

    [InverseProperty("UnitParentNavigation")]
    public virtual ICollection<Unit> InverseUnitParentNavigation { get; set; } = new List<Unit>();

    [ForeignKey("OrgId")]
    [InverseProperty("Units")]
    public virtual Org Org { get; set; } = null!;

    [InverseProperty("Unit")]
    public virtual ICollection<Reminderunit> Reminderunits { get; set; } = new List<Reminderunit>();

    [InverseProperty("Unit")]
    public virtual ICollection<Taskunitassignment> Taskunitassignments { get; set; } = new List<Taskunitassignment>();

    [ForeignKey("UnitParent")]
    [InverseProperty("InverseUnitParentNavigation")]
    public virtual Unit? UnitParentNavigation { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<Unituser> Unitusers { get; set; } = new List<Unituser>();

    [InverseProperty("Unit")]
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
