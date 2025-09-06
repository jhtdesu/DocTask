using DocTask.Core.Dtos.Tasks;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasks();
    Task<TaskDto?> GetTaskById(int id);
    Task<TaskDto> CreateTask(CreateMainTaskRequest request);
    Task<TaskDto> CreateTaskWithDetails(CreateTaskRequest request);
    Task<TaskDto?> UpdateTask(int id, UpdateTaskRequest request);
    Task<bool> DeleteTask(int id);
    
    // Subtask methods
    Task<List<TaskDto>> GetSubtasksByParentId(int parentTaskId);
    Task<TaskDto?> GetSubtaskById(int parentTaskId, int subtaskId);
    Task<TaskDto> CreateSubtask(int parentTaskId, CreateTaskRequest request);
    Task<TaskDto?> UpdateSubtask(int parentTaskId, int subtaskId, UpdateTaskRequest request);
    Task<bool> DeleteSubtask(int parentTaskId, int subtaskId);
}