using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTask.Core.Models;

[Table("position")]
public partial class Position
{
    [Key]
    [Column("positionId")]
    public int PositionId { get; set; }

    [Column("positionName")]
    [StringLength(255)]
    public string PositionName { get; set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
