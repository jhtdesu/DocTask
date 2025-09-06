using Microsoft.AspNetCore.Http;

namespace DocTask.Core.Dtos.UploadFile;

public class UploadFileDto
{
    public int FileId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
}

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
    public string? Description { get; set; }
}
