using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("userrole")]
[Index("Roleid", Name = "fk_userrole_role")]
[Index("Userid", Name = "fk_userrole_user")]
public partial class Userrole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userid")]
    public string Userid { get; set; } = null!;

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("createdat")]
    public DateTime Createdat { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Userroles")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Userroles")]
    public virtual ApplicationUser User { get; set; } = null!;
}
