using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTask.Core.Models;

[Table("frequency")]
public partial class Frequency
{
    [Key]
    [Column("frequencyId")]
    public int FrequencyId { get; set; }

    [Column("frequencyType")]
    [StringLength(50)]
    public string FrequencyType { get; set; } = null!;

    [Column("frequencyDetail")]
    [StringLength(100)]
    public string? FrequencyDetail { get; set; }

    [Column("intervalValue")]
    public int IntervalValue { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Frequency")]
    public virtual ICollection<FrequencyDetail> FrequencyDetails { get; set; } = new List<FrequencyDetail>();

    [InverseProperty("Frequency")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
