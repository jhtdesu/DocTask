using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DocTask.Service.Services;

public class UploadFileService : IUploadFileService
{
    private readonly IUploadFileRepository _uploadFileRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _uploadPath;

    public UploadFileService(IUploadFileRepository uploadFileRepository, IWebHostEnvironment webHostEnvironment)
    {
        _uploadFileRepository = uploadFileRepository;
        _webHostEnvironment = webHostEnvironment;
        _uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "UploadFile");
        
        // Ensure the upload directory exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<UploadFileDto> UploadFileAsync(UploadFileRequest request, string userId)
    {
        if (request.File == null || request.File.Length == 0)
            throw new ArgumentException("No file provided");

        // Validate file size (e.g., 10MB limit)
        const long maxFileSize = 10 * 1024 * 1024; // 10MB
        if (request.File.Length > maxFileSize)
            throw new ArgumentException("File size exceeds the maximum limit of 10MB");

        // Generate unique filename
        var fileExtension = Path.GetExtension(request.File.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        // Save file to disk
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        // Create database record
        var uploadFile = new Uploadfile
        {
            FileName = request.File.FileName,
            FilePath = filePath,
            UploadedBy = userId,
            UploadedAt = DateTime.UtcNow
        };

        var savedFile = await _uploadFileRepository.CreateAsync(uploadFile);

        return new UploadFileDto
        {
            FileId = savedFile.FileId,
            FileName = savedFile.FileName,
            FilePath = savedFile.FilePath,
            UploadedBy = savedFile.UploadedBy,
            UploadedAt = savedFile.UploadedAt,
            FileSize = request.File.Length,
            ContentType = request.File.ContentType
        };
    }

    public async Task<UploadFileDto?> GetFileByIdAsync(int fileId)
    {
        var uploadFile = await _uploadFileRepository.GetByIdAsync(fileId);
        if (uploadFile == null)
            return null;

        var fileInfo = new FileInfo(uploadFile.FilePath);
        
        return new UploadFileDto
        {
            FileId = uploadFile.FileId,
            FileName = uploadFile.FileName,
            FilePath = uploadFile.FilePath,
            UploadedBy = uploadFile.UploadedBy,
            UploadedAt = uploadFile.UploadedAt,
            FileSize = fileInfo.Exists ? fileInfo.Length : 0,
            ContentType = GetContentType(uploadFile.FileName)
        };
    }

    public async Task<List<UploadFileDto>> GetFilesByUserIdAsync(string userId)
    {
        var uploadFiles = await _uploadFileRepository.GetByUserIdAsync(userId);
        
        return uploadFiles.Select(uploadFile =>
        {
            var fileInfo = new FileInfo(uploadFile.FilePath);
            return new UploadFileDto
            {
                FileId = uploadFile.FileId,
                FileName = uploadFile.FileName,
                FilePath = uploadFile.FilePath,
                UploadedBy = uploadFile.UploadedBy,
                UploadedAt = uploadFile.UploadedAt,
                FileSize = fileInfo.Exists ? fileInfo.Length : 0,
                ContentType = GetContentType(uploadFile.FileName)
            };
        }).ToList();
    }

    public async Task<PaginationResponse<UploadFileDto>> GetFilesByUserIdPaginated(string userId, PaginationRequest request)
    {
        var (items, totalCount) = await _uploadFileRepository.GetByUserIdPaginated(userId, request);
        
        var uploadFileDtos = items.Select(uploadFile =>
        {
            var fileInfo = new FileInfo(uploadFile.FilePath);
            return new UploadFileDto
            {
                FileId = uploadFile.FileId,
                FileName = uploadFile.FileName,
                FilePath = uploadFile.FilePath,
                UploadedBy = uploadFile.UploadedBy,
                UploadedAt = uploadFile.UploadedAt,
                FileSize = fileInfo.Exists ? fileInfo.Length : 0,
                ContentType = GetContentType(uploadFile.FileName)
            };
        }).ToList();
        
        return new PaginationResponse<UploadFileDto>
        {
            Data = uploadFileDtos,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public async Task<bool> DeleteFileAsync(int fileId, string userId)
    {
        var uploadFile = await _uploadFileRepository.GetByIdAndUserIdAsync(fileId, userId);
        if (uploadFile == null)
            return false;

        // Delete physical file
        if (File.Exists(uploadFile.FilePath))
        {
            File.Delete(uploadFile.FilePath);
        }

        // Delete database record
        return await _uploadFileRepository.DeleteAsync(fileId);
    }

    public async Task<byte[]?> GetFileContentAsync(int fileId)
    {
        var uploadFile = await _uploadFileRepository.GetByIdAsync(fileId);
        if (uploadFile == null || !File.Exists(uploadFile.FilePath))
            return null;

        return await File.ReadAllBytesAsync(uploadFile.FilePath);
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }
}
