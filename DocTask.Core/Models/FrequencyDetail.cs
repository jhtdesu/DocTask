using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("frequency_detail")]
[Index("FrequencyId", Name = "fk_frequency_detail_frequency")]
public partial class FrequencyDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("frequencyId")]
    public int FrequencyId { get; set; }

    [Column("dayOfWeek")]
    public sbyte? DayOfWeek { get; set; }

    [Column("dayOfMonth")]
    public sbyte? DayOfMonth { get; set; }

    [ForeignKey("FrequencyId")]
    [InverseProperty("FrequencyDetails")]
    public virtual Frequency Frequency { get; set; } = null!;
}
