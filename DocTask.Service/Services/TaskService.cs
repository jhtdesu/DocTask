using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Service.Mappers;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Service.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskDto>> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllTasks();
        return tasks.Select(TaskMapper.ToDto).ToList();
    }

    public async Task<TaskDto?> GetTaskById(int id)
    {
        var task = await _taskRepository.GetTaskById(id);
        return task != null ? TaskMapper.ToDto(task) : null;
    }

    public async Task<TaskDto> CreateTask(CreateMainTaskRequest request)
    {
        var task = TaskMapper.ToEntity(request);
        var createdTask = await _taskRepository.CreateTask(task);
        return TaskMapper.ToDto(createdTask);
    }

    public async Task<TaskDto> CreateTaskWithDetails(CreateTaskRequest request)
    {
        var task = TaskMapper.ToEntity(request);
        var createdTask = await _taskRepository.CreateTask(task);
        return TaskMapper.ToDto(createdTask);
    }

    public async Task<TaskDto?> UpdateTask(int id, UpdateTaskRequest request)
    {
        var existingTask = await _taskRepository.GetTaskById(id);
        if (existingTask == null)
            return null;

        TaskMapper.UpdateEntity(existingTask, request);
        var updatedTask = await _taskRepository.UpdateTask(existingTask);
        return updatedTask != null ? TaskMapper.ToDto(updatedTask) : null;
    }

    public async Task<bool> DeleteTask(int id)
    {
        return await _taskRepository.DeleteTask(id);
    }

    // Subtask methods
    public async Task<List<TaskDto>> GetSubtasksByParentId(int parentTaskId)
    {
        var subtasks = await _taskRepository.GetSubtasksByParentId(parentTaskId);
        return subtasks.Select(TaskMapper.ToDto).ToList();
    }

    public async Task<TaskDto?> GetSubtaskById(int parentTaskId, int subtaskId)
    {
        var subtask = await _taskRepository.GetSubtaskById(parentTaskId, subtaskId);
        return subtask != null ? TaskMapper.ToDto(subtask) : null;
    }

    public async Task<TaskDto> CreateSubtask(int parentTaskId, CreateTaskRequest request)
    {
        var subtask = TaskMapper.ToEntity(request);
        subtask.ParentTaskId = parentTaskId; // Ensure the parent task ID is set
        var createdSubtask = await _taskRepository.CreateSubtask(subtask);
        return TaskMapper.ToDto(createdSubtask);
    }

    public async Task<TaskDto?> UpdateSubtask(int parentTaskId, int subtaskId, UpdateTaskRequest request)
    {
        var existingSubtask = await _taskRepository.GetSubtaskById(parentTaskId, subtaskId);
        if (existingSubtask == null)
            return null;

        TaskMapper.UpdateEntity(existingSubtask, request);
        var updatedSubtask = await _taskRepository.UpdateSubtask(parentTaskId, existingSubtask);
        return updatedSubtask != null ? TaskMapper.ToDto(updatedSubtask) : null;
    }

    public async Task<bool> DeleteSubtask(int parentTaskId, int subtaskId)
    {
        return await _taskRepository.DeleteSubtask(parentTaskId, subtaskId);
    }
}