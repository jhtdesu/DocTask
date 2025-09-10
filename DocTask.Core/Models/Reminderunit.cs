using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("reminderunit")]
[Index("Reminderid", Name = "fk_reminder")]
[Index("Unitid", Name = "fk_unit")]
public partial class Reminderunit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reminderid")]
    public int Reminderid { get; set; }

    [Column("unitid")]
    public int Unitid { get; set; }

    [ForeignKey("Reminderid")]
    [InverseProperty("Reminderunits")]
    public virtual Reminder Reminder { get; set; } = null!;

    [ForeignKey("Unitid")]
    [InverseProperty("Reminderunits")]
    public virtual Unit Unit { get; set; } = null!;
}
