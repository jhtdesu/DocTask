using DocTask.Core.Models;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<List<TaskEntity>> GetAllTasks();
    Task<TaskEntity?> GetTaskById(int id);
    Task<TaskEntity> CreateTask(TaskEntity task);
    Task<TaskEntity?> UpdateTask(TaskEntity task);
    Task<bool> DeleteTask(int id);
    Task<bool> TaskExists(int id);
    
    // Subtask methods
    Task<List<TaskEntity>> GetSubtasksByParentId(int parentTaskId);
    Task<TaskEntity?> GetSubtaskById(int parentTaskId, int subtaskId);
    Task<TaskEntity> CreateSubtask(TaskEntity subtask);
    Task<TaskEntity?> UpdateSubtask(int parentTaskId, TaskEntity subtask);
    Task<bool> DeleteSubtask(int parentTaskId, int subtaskId);
}