using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Models;

[Table("uploadfile")]
[Index("UploadedBy", Name = "fkUploadFileUploadedBy")]
public partial class Uploadfile
{
    [Key]
    [Column("fileId")]
    public int FileId { get; set; }

    [Column("fileName")]
    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [Column("filePath")]
    [StringLength(255)]
    public string FilePath { get; set; } = null!;

    [Column("uploadedBy")]
    public int? UploadedBy { get; set; }

    [Column("uploadedAt")]
    public DateTime UploadedAt { get; set; }

    [InverseProperty("ReportFileNavigation")]
    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    [InverseProperty("AttachedFileNavigation")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [ForeignKey("UploadedBy")]
    [InverseProperty("Uploadfiles")]
    public virtual User? UploadedByNavigation { get; set; }
}
