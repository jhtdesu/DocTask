using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasks();
        return Ok(new ApiResponse<List<TaskDto>> { Success = true, Data = tasks });
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
    public async Task<IActionResult> GetSubtasksByParentId(int parentTaskId)
    {
        var subtasks = await _taskService.GetSubtasksByParentId(parentTaskId);
        return Ok(new ApiResponse<List<TaskDto>> { Success = true, Data = subtasks });
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
    public async Task<IActionResult> CreateSubtask(int parentTaskId, [FromBody] CreateSubtaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var subtask = await _taskService.CreateSubtask(parentTaskId, request);
            return CreatedAtAction(nameof(GetSubtaskById), new { parentTaskId, subtaskId = subtask.TaskId }, 
                new ApiResponse<TaskDto> { Success = true, Data = subtask });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = $"Error creating subtask: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpPut("{parentTaskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> UpdateSubtask(int parentTaskId, int subtaskId, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = "Invalid request data" });
        }

        try
        {
            var subtask = await _taskService.UpdateSubtask(parentTaskId, subtaskId, request);
            if (subtask == null)
            {
                return NotFound(new ApiResponse<TaskDto> { Success = false, Error = "Subtask not found" });
            }
            return Ok(new ApiResponse<TaskDto> { Success = true, Data = subtask });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TaskDto> { Success = false, Error = $"Error updating subtask: {ex.Message}" });
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