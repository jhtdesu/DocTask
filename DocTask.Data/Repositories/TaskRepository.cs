using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskEntity>> GetAllTasks()
    {
        return await _context.Tasks
            .Where(t => t.ParentTaskId == null)
            .ToListAsync();
    }

    public async Task<TaskEntity?> GetTaskById(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskEntity> CreateTask(TaskEntity task)
    {
        task.CreatedAt = DateTime.UtcNow;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskEntity?> UpdateTask(TaskEntity task)
    {
        var existingTask = await _context.Tasks.FindAsync(task.TaskId);
        if (existingTask == null)
            return null;

        _context.Entry(existingTask).CurrentValues.SetValues(task);
        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TaskExists(int id)
    {
        return await _context.Tasks.AnyAsync(t => t.TaskId == id);
    }

    // Subtask methods
    public async Task<List<TaskEntity>> GetSubtasksByParentId(int parentTaskId)
    {
        return await _context.Tasks
            .Where(t => t.ParentTaskId == parentTaskId)
            .ToListAsync();
    }

    public async Task<TaskEntity?> GetSubtaskById(int parentTaskId, int subtaskId)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == subtaskId && t.ParentTaskId == parentTaskId);
    }

    public async Task<TaskEntity> CreateSubtask(TaskEntity subtask)
    {
        subtask.CreatedAt = DateTime.UtcNow;
        _context.Tasks.Add(subtask);
        await _context.SaveChangesAsync();
        return subtask;
    }

    public async Task<TaskEntity?> UpdateSubtask(int parentTaskId, TaskEntity subtask)
    {
        var existingSubtask = await _context.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == subtask.TaskId && t.ParentTaskId == parentTaskId);
        
        if (existingSubtask == null)
            return null;

        _context.Entry(existingSubtask).CurrentValues.SetValues(subtask);
        await _context.SaveChangesAsync();
        return existingSubtask;
    }

    public async Task<bool> DeleteSubtask(int parentTaskId, int subtaskId)
    {
        var subtask = await _context.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == subtaskId && t.ParentTaskId == parentTaskId);
        
        if (subtask == null)
            return false;

        _context.Tasks.Remove(subtask);
        await _context.SaveChangesAsync();
        return true;
    }
}