using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Enums;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<List<TaskEntity>> GetAllTasks();
    Task<(List<TaskEntity> items, int totalCount)> GetTasksPaginated(PaginationRequest request);
    Task<TaskEntity?> GetTaskById(int id);
    Task<TaskEntity> CreateTask(TaskEntity task);
    Task<TaskEntity?> UpdateTask(TaskEntity task);
    Task<bool> DeleteTask(int id);
    Task<bool> TaskExists(int id);
    
    // Subtask methods
    Task<List<TaskEntity>> GetSubtasksByParentId(int parentTaskId);
    Task<(List<TaskEntity> items, int totalCount)> GetSubtasksPaginated(int parentTaskId, PaginationRequest request);
    Task<TaskEntity?> GetSubtaskById(int parentTaskId, int subtaskId);
    Task<TaskEntity> CreateSubtask(TaskEntity subtask);
    Task<TaskEntity?> UpdateSubtask(int parentTaskId, TaskEntity subtask);
    Task<TaskEntity?> UpdateSubtaskWithAssignments(int parentTaskId, TaskEntity subtask, List<string> userIds);
    Task<bool> DeleteSubtask(int parentTaskId, int subtaskId);
    
    // Task assignment methods
    Task<TaskEntity> CreateSubtaskWithAssignments(TaskEntity subtask, List<string> userIds);
    Task<bool> AssignUsersToTask(int taskId, List<string> userIds);
    Task<bool> RemoveUserFromTask(int taskId, string userId);
    Task<List<ApplicationUser>> GetTaskAssignees(int taskId);
}