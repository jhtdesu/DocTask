using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/upload")]
[Authorize]
public class UploadFileController : ControllerBase
{
    private readonly IUploadFileService _uploadFileService;

    public UploadFileController(IUploadFileService uploadFileService)
    {
        _uploadFileService = uploadFileService;
    }

    [HttpPost("file")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UploadFileDto> 
            { 
                Success = false, 
                Error = "Invalid request data" 
            });
        }

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<UploadFileDto> 
                { 
                    Success = false, 
                    Error = "User not authenticated" 
                });
            }

            var result = await _uploadFileService.UploadFileAsync(request, userId);
            return Ok(new ApiResponse<UploadFileDto> 
            { 
                Success = true, 
                Data = result, 
                Message = "File uploaded successfully" 
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<UploadFileDto> 
            { 
                Success = false, 
                Error = ex.Message 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UploadFileDto> 
            { 
                Success = false, 
                Error = $"Error uploading file: {ex.Message}" 
            });
        }
    }

    [HttpGet("file/{fileId}")]
    public async Task<IActionResult> GetFile(int fileId)
    {
        try
        {
            var file = await _uploadFileService.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return NotFound(new ApiResponse<UploadFileDto> 
                { 
                    Success = false, 
                    Error = "File not found" 
                });
            }

            return Ok(new ApiResponse<UploadFileDto> 
            { 
                Success = true, 
                Data = file 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UploadFileDto> 
            { 
                Success = false, 
                Error = $"Error retrieving file: {ex.Message}" 
            });
        }
    }

    [HttpGet("files")]
    public async Task<IActionResult> GetUserFiles()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<List<UploadFileDto>> 
                { 
                    Success = false, 
                    Error = "User not authenticated" 
                });
            }

            var files = await _uploadFileService.GetFilesByUserIdAsync(userId);
            return Ok(new ApiResponse<List<UploadFileDto>> 
            { 
                Success = true, 
                Data = files 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<UploadFileDto>> 
            { 
                Success = false, 
                Error = $"Error retrieving files: {ex.Message}" 
            });
        }
    }

    [HttpGet("file/{fileId}/download")]
    public async Task<IActionResult> DownloadFile(int fileId)
    {
        try
        {
            var file = await _uploadFileService.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return NotFound(new ApiResponse<object> 
                { 
                    Success = false, 
                    Error = "File not found" 
                });
            }

            var fileContent = await _uploadFileService.GetFileContentAsync(fileId);
            if (fileContent == null)
            {
                return NotFound(new ApiResponse<object> 
                { 
                    Success = false, 
                    Error = "File content not found" 
                });
            }

            return File(fileContent, file.ContentType, file.FileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> 
            { 
                Success = false, 
                Error = $"Error downloading file: {ex.Message}" 
            });
        }
    }

    [HttpDelete("file/{fileId}")]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<bool> 
                { 
                    Success = false, 
                    Error = "User not authenticated" 
                });
            }

            var result = await _uploadFileService.DeleteFileAsync(fileId, userId);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> 
                { 
                    Success = false, 
                    Error = "File not found or access denied" 
                });
            }

            return Ok(new ApiResponse<bool> 
            { 
                Success = true, 
                Data = true, 
                Message = "File deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<bool> 
            { 
                Success = false, 
                Error = $"Error deleting file: {ex.Message}" 
            });
        }
    }
}
