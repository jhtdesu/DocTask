using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("org")]
[Index("ParentOrgId", Name = "fkOrgParent")]
public partial class Org
{
    [Key]
    [Column("orgId")]
    public int OrgId { get; set; }

    [Column("orgName")]
    [StringLength(100)]
    public string OrgName { get; set; } = null!;

    [Column("parentOrgId")]
    public int? ParentOrgId { get; set; }

    [Column("createdAt", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("ParentOrg")]
    public virtual ICollection<Org> InverseParentOrg { get; set; } = new List<Org>();

    [ForeignKey("ParentOrgId")]
    [InverseProperty("InverseParentOrg")]
    public virtual Org? ParentOrg { get; set; }

    [InverseProperty("Org")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [InverseProperty("Org")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();

    [InverseProperty("Org")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
