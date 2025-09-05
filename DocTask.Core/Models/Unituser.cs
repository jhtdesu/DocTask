using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("unituser")]
[Index("UnitId", Name = "fkUnitUserUnit")]
[Index("UserId", Name = "fkUnitUserUser")]
public partial class Unituser
{
    [Key]
    [Column("unitUserId")]
    public int UnitUserId { get; set; }

    [Column("unitId")]
    public int UnitId { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("position", TypeName = "text")]
    public string? Position { get; set; }

    [Column("level")]
    public int? Level { get; set; }

    [ForeignKey("UnitId")]
    [InverseProperty("Unitusers")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Unitusers")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("UnitUser")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
