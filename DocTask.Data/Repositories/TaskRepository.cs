using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Enums;
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
            .Include(t => t.Period)
            .Where(t => t.ParentTaskId == null)
            .ToListAsync();
    }

    public async Task<(List<TaskEntity> items, int totalCount)> GetTasksPaginated(PaginationRequest request)
    {
        var query = _context.Tasks
            .Include(t => t.Period)
            .Where(t => t.ParentTaskId == null);

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(request.SearchTerm) || 
                                   (t.Description != null && t.Description.Contains(request.SearchTerm)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "name":
                    query = request.SortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title);
                    break;
                case "createdat":
                    query = request.SortDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                    break;
                case "duedate":
                    query = request.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate);
                    break;
                case "priority":
                    query = request.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority);
                    break;
                default:
                    query = query.OrderBy(t => t.TaskId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(t => t.TaskId);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<TaskEntity?> GetTaskById(int id)
    {
        return await _context.Tasks
            .Include(t => t.Period)
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == id);
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
            .Include(t => t.Period)
            .Include(t => t.Users)
            .Where(t => t.ParentTaskId == parentTaskId)
            .ToListAsync();
    }

    public async Task<(List<TaskEntity> items, int totalCount)> GetSubtasksPaginated(int parentTaskId, PaginationRequest request)
    {
        var query = _context.Tasks
            .Include(t => t.Period)
            .Include(t => t.Users)
            .Where(t => t.ParentTaskId == parentTaskId);

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(request.SearchTerm) || 
                                   (t.Description != null && t.Description.Contains(request.SearchTerm)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "name":
                    query = request.SortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title);
                    break;
                case "createdat":
                    query = request.SortDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                    break;
                case "duedate":
                    query = request.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate);
                    break;
                case "priority":
                    query = request.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority);
                    break;
                default:
                    query = query.OrderBy(t => t.TaskId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(t => t.TaskId);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<TaskEntity?> GetSubtaskById(int parentTaskId, int subtaskId)
    {
        return await _context.Tasks
            .Include(t => t.Period)
            .Include(t => t.Users)
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

    public async Task<TaskEntity?> UpdateSubtaskWithAssignments(int parentTaskId, TaskEntity subtask, List<string> userIds)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingSubtask = await _context.Tasks
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.TaskId == subtask.TaskId && t.ParentTaskId == parentTaskId);
            
            if (existingSubtask == null)
                return null;

            // Update basic properties
            _context.Entry(existingSubtask).CurrentValues.SetValues(subtask);

            // Clear existing user assignments
            existingSubtask.Users.Clear();

            // Add new user assignments
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                existingSubtask.Users.Add(user);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return existingSubtask;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    // Task assignment methods
    public async Task<TaskEntity> CreateSubtaskWithAssignments(TaskEntity subtask, List<string> userIds)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Create the subtask
            subtask.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(subtask);
            await _context.SaveChangesAsync();

            // Reload the subtask to get proper tracking for the many-to-many relationship
            var createdSubtask = await _context.Tasks
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.TaskId == subtask.TaskId);

            if (createdSubtask != null)
            {
                // Add user assignments using the existing many-to-many relationship
                var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
                foreach (var user in users)
                {
                    createdSubtask.Users.Add(user);
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return createdSubtask ?? subtask;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> AssignUsersToTask(int taskId, List<string> userIds)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var task = await _context.Tasks
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return false;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                if (!task.Users.Any(u => u.Id == user.Id))
                {
                    task.Users.Add(user);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> RemoveUserFromTask(int taskId, string userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == taskId);

        if (task == null)
            return false;

        var user = task.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return false;

        task.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ApplicationUser>> GetTaskAssignees(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == taskId);

        return task?.Users.ToList() ?? new List<ApplicationUser>();
    }
}