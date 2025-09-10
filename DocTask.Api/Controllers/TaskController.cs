using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] PaginationRequest? request = null)
    {
        if (request != null)
        {
            var tasks = await _taskService.GetTasksPaginated(request);
            return Ok(new PaginatedApiResponse<TaskDto>(tasks, "Tasks retrieved successfully"));
        }
        else
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(new ApiResponse<List<TaskDto>> { Success = true, Data = tasks });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskById(id);
        if (task == null)
        {
            return NotFound(new ApiResponse<TaskDto> { Success = false, Error = "Task not found" });
        }
        return Ok(new ApiResponse<TaskDto> { Success = true, Data = task });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateMainTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var task = await _taskService.CreateTask(request);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, new ApiResponse<TaskDto> { Success = true, Data = task });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = $"Error creating task: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var task = await _taskService.UpdateTask(id, request);
            if (task == null)
            {
                return NotFound(new ApiResponse<TaskDto> { Success = false, Error = "Task not found" });
            }
            return Ok(new ApiResponse<TaskDto> { Success = true, Data = task });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = $"Error updating task: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var result = await _taskService.DeleteTask(id);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Error = "Task not found" });
            }
            return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Task deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool> { Success = false, Error = $"Error deleting task: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpGet("{parentTaskId}/subtasks")]
    public async Task<IActionResult> GetSubtasksByParentId(int parentTaskId, [FromQuery] PaginationRequest? request = null)
    {
        if (request != null)
        {
            var subtasks = await _taskService.GetSubtasksPaginated(parentTaskId, request);
            return Ok(new PaginatedApiResponse<TaskDto>(subtasks, "Subtasks retrieved successfully"));
        }
        else
        {
            var subtasks = await _taskService.GetSubtasksByParentId(parentTaskId);
            return Ok(new ApiResponse<List<TaskDto>> { Success = true, Data = subtasks });
        }
    }

    [Authorize]
    [HttpGet("{parentTaskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> GetSubtaskById(int parentTaskId, int subtaskId)
    {
        var subtask = await _taskService.GetSubtaskById(parentTaskId, subtaskId);
        if (subtask == null)
        {
            return NotFound(new ApiResponse<TaskDto> { Success = false, Error = "Subtask not found" });
        }
        return Ok(new ApiResponse<TaskDto> { Success = true, Data = subtask });
    }

    [Authorize]
    [HttpPost("{parentTaskId}/subtasks")]
    public async Task<IActionResult> CreateSubtask(int parentTaskId, [FromBody] CreateSubtaskWithAssignmentsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            // Get the current user ID from the claims
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new ApiResponse<TaskDto> { Success = false, Error = "User not authenticated" });
            }

            // Set the assigner ID to the current user
            request.AssignerId = currentUserId;


            var subtask = await _taskService.CreateSubtaskWithAssignments(parentTaskId, request);
            return CreatedAtAction(nameof(GetSubtaskById), new { parentTaskId, subtaskId = subtask.TaskId }, 
                new ApiResponse<TaskDto> { Success = true, Data = subtask, Message = "Subtask created with user assignments successfully" });
        }
        catch (Exception ex)
        {
            // Enhanced error logging for debugging
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return BadRequest(new ApiResponse<TaskDto> { 
                Success = false, 
                Error = $"Error creating subtask: {errorMessage}" 
            });
        }
    }

    [Authorize]
    [HttpPut("{parentTaskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> UpdateSubtask(int parentTaskId, int subtaskId, [FromBody] UpdateSubtaskWithAssignmentsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            // Get the current user ID from the claims
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new ApiResponse<TaskDto> { Success = false, Error = "User not authenticated" });
            }

            // Set the assigner ID to the current user if not provided
            if (string.IsNullOrEmpty(request.AssignerId))
            {
                request.AssignerId = currentUserId;
            }


            var subtask = await _taskService.UpdateSubtaskWithAssignments(parentTaskId, subtaskId, request);
            if (subtask == null)
            {
                return NotFound(new ApiResponse<TaskDto> { Success = false, Error = "Subtask not found" });
            }
            return Ok(new ApiResponse<TaskDto> { Success = true, Data = subtask, Message = "Subtask updated with user assignments successfully" });
        }
        catch (Exception ex)
        {
            // Enhanced error logging for debugging
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return BadRequest(new ApiResponse<TaskDto> { 
                Success = false, 
                Error = $"Error updating subtask: {errorMessage}" 
            });
        }
    }

    [Authorize]
    [HttpDelete("{parentTaskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> DeleteSubtask(int parentTaskId, int subtaskId)
    {
        try
        {
            var result = await _taskService.DeleteSubtask(parentTaskId, subtaskId);
            if (!result)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Error = "Subtask not found" });
            }
            return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Subtask deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool> { Success = false, Error = $"Error deleting subtask: {ex.Message}" });
        }
    }

}