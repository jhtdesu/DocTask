using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Models;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Service.Mappers;

public static class TaskMapper
{
    public static TaskDto ToDto(TaskEntity task)
    {
        return new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            AssignerId = task.AssignerId,
            AssigneeId = task.AssigneeId,
            OrgId = task.OrgId,
            PeriodId = task.PeriodId,
            AttachedFile = task.AttachedFile,
            Status = task.Status,
            Priority = task.Priority,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UnitId = task.UnitId,
            FrequencyId = task.FrequencyId,
            Percentagecomplete = task.Percentagecomplete,
            ParentTaskId = task.ParentTaskId,
            FrequencyDate = null
        };
    }

    public static TaskEntity ToEntity(CreateMainTaskRequest request)
    {
        return new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            Status = "Pending", // Default status
            Priority = "Medium", // Default priority
            Percentagecomplete = 0 // Default completion percentage
        };
    }

    public static TaskEntity ToEntity(CreateTaskRequest request)
    {
        return new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            AssignerId = string.IsNullOrWhiteSpace(request.AssignerId) ? null : request.AssignerId,
            AssigneeId = string.IsNullOrWhiteSpace(request.AssigneeId) ? null : request.AssigneeId,
            OrgId = request.OrgId,
            PeriodId = request.PeriodId,
            AttachedFile = request.AttachedFile,
            Status = request.Status,
            Priority = request.Priority,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            UnitId = request.UnitId,
            FrequencyId = request.FrequencyId,
            Percentagecomplete = request.Percentagecomplete,
            ParentTaskId = request.ParentTaskId
        };
    }

    public static TaskEntity ToEntity(CreateSubtaskRequest request)
    {
        return new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            FrequencyId = request.FrequencyId,
            PeriodId = request.PeriodId,
            CreatedAt = DateTime.UtcNow,
            Status = "Pending", // Default status
            Priority = "Medium", // Default priority
            Percentagecomplete = 0 // Default completion percentage
        };
    }

    public static void UpdateEntity(TaskEntity existingTask, UpdateTaskRequest request)
    {
        if (request.Title != null)
            existingTask.Title = request.Title;
        
        if (request.Description != null)
            existingTask.Description = request.Description;
        
        if (request.StartDate.HasValue)
            existingTask.StartDate = request.StartDate;
        
        if (request.DueDate.HasValue)
            existingTask.DueDate = request.DueDate;
    }
}