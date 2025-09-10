using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.UploadFile;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DockTask.Api.Extensions;

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
        var userId = User.GetUserId();
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

    [HttpGet("file/{fileId}")]
    public async Task<IActionResult> GetFile(int fileId)
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

    [HttpGet("files")]
    public async Task<IActionResult> GetUserFiles([FromQuery] PaginationRequest? request = null)
    {
        var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            if (request != null)
            {
                return Unauthorized(new PaginatedApiResponse<UploadFileDto>(false, "User not authenticated"));
            }
            else
            {
                return Unauthorized(new ApiResponse<List<UploadFileDto>> 
                { 
                    Success = false, 
                    Error = "User not authenticated" 
                });
            }
        }

        if (request != null)
        {
            var files = await _uploadFileService.GetFilesByUserIdPaginated(userId, request);
            return Ok(new PaginatedApiResponse<UploadFileDto>(files, "Files retrieved successfully"));
        }
        else
        {
            var files = await _uploadFileService.GetFilesByUserIdAsync(userId);
            return Ok(new ApiResponse<List<UploadFileDto>> 
            { 
                Success = true, 
                Data = files 
            });
        }
    }

    [HttpGet("file/{fileId}/download")]
    public async Task<IActionResult> DownloadFile(int fileId)
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

    [HttpDelete("file/{fileId}")]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        var userId = User.GetUserId();
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
}
