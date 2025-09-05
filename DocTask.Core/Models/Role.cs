using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("role")]
[Index("Rolename", Name = "rolename", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("rolename")]
    [StringLength(100)]
    public string Rolename { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("createdat", TypeName = "timestamp")]
    public DateTime Createdat { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
