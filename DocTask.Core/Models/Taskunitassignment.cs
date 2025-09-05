using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("taskunitassignment")]
[Index("TaskId", Name = "TaskId")]
[Index("UnitId", Name = "UnitId")]
public partial class Taskunitassignment
{
    [Key]
    public int TaskUnitAssignmentId { get; set; }

    public int TaskId { get; set; }

    public int UnitId { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("Taskunitassignments")]
    public virtual Task Task { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Taskunitassignments")]
    public virtual Unit Unit { get; set; } = null!;
}
