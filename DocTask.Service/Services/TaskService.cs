using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Dtos.Frequency;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using DocTask.Service.Mappers;
using TaskEntity = DocTask.Core.Models.Task;
using System.Linq;

namespace DocTask.Service.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IFrequencyService _frequencyService;
    private readonly IPeriodRepository _periodRepository;

    public TaskService(ITaskRepository taskRepository, IFrequencyService frequencyService, IPeriodRepository periodRepository)
    {
        _taskRepository = taskRepository;
        _frequencyService = frequencyService;
        _periodRepository = periodRepository;
    }

    private static PaginationResponse<TDto> BuildPaginationResponse<TDto>(IReadOnlyList<TDto> items, PaginationRequest request, int totalCount)
    {
        return new PaginationResponse<TDto>
        {
            Data = items.ToList(),
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    private async Task<TaskEntity> EnsureParentTaskExists(int parentTaskId)
    {
        var parentTask = await _taskRepository.GetTaskById(parentTaskId);
        if (parentTask == null)
        {
            throw new NotFoundException($"Parent task with ID {parentTaskId} not found");
        }
        return parentTask;
    }

    public async Task<List<TaskDto>> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllTasks();
        return tasks.Select(TaskMapper.ToDto).ToList();
    }

    public async Task<PaginationResponse<TaskDto>> GetTasksPaginated(PaginationRequest request)
    {
        var (items, totalCount) = await _taskRepository.GetTasksPaginated(request);
        var taskDtos = items.Select(TaskMapper.ToDto).ToList();
        return BuildPaginationResponse(taskDtos, request, totalCount);
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

    public async Task<List<SubtaskWithPeriodsDto>> GetSubtasksWithNearbyPeriods(int parentTaskId, int secondsTolerance = 5)
    {
        var subtasks = await _taskRepository.GetSubtasksByParentId(parentTaskId);
        var results = new List<SubtaskWithPeriodsDto>();
        foreach (var st in subtasks)
        {
            var dto = TaskMapper.ToDto(st);
            var periods = new List<DocTask.Core.Dtos.Period.PeriodDto>();
            if (st.PeriodId.HasValue)
            {
                var first = await _periodRepository.GetPeriodById(st.PeriodId.Value);
                if (first != null)
                {
                    var nearby = await _periodRepository.GetPeriodsNearCreatedAt(first.CreatedAt, secondsTolerance);
                    periods = nearby.Select(DocTask.Service.Mappers.PeriodMapper.ToDto).ToList();
                }
            }
            results.Add(new SubtaskWithPeriodsDto { Subtask = dto, Periods = periods });
        }
        return results;
    }

    public async Task<PaginationResponse<TaskDto>> GetSubtasksPaginated(int parentTaskId, PaginationRequest request)
    {
        var (items, totalCount) = await _taskRepository.GetSubtasksPaginated(parentTaskId, request);
        var subtaskDtos = items.Select(TaskMapper.ToDto).ToList();
        return BuildPaginationResponse(subtaskDtos, request, totalCount);
    }

    public async Task<PaginationResponse<SubtaskWithPeriodsDto>> GetSubtasksWithNearbyPeriodsPaginated(int parentTaskId, PaginationRequest request, int secondsTolerance = 5)
    {
        var (items, totalCount) = await _taskRepository.GetSubtasksPaginated(parentTaskId, request);
        var list = new List<SubtaskWithPeriodsDto>();
        foreach (var st in items)
        {
            var dto = TaskMapper.ToDto(st);
            var periods = new List<DocTask.Core.Dtos.Period.PeriodDto>();
            if (st.PeriodId.HasValue)
            {
                var first = await _periodRepository.GetPeriodById(st.PeriodId.Value);
                if (first != null)
                {
                    var nearby = await _periodRepository.GetPeriodsNearCreatedAt(first.CreatedAt, secondsTolerance);
                    periods = nearby.Select(DocTask.Service.Mappers.PeriodMapper.ToDto).ToList();
                }
            }
            list.Add(new SubtaskWithPeriodsDto { Subtask = dto, Periods = periods });
        }
        return new PaginationResponse<SubtaskWithPeriodsDto>
        {
            Data = list,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public async Task<TaskDto?> GetSubtaskById(int parentTaskId, int subtaskId)
    {
        var subtask = await _taskRepository.GetSubtaskById(parentTaskId, subtaskId);
        return subtask != null ? TaskMapper.ToDto(subtask) : null;
    }

    public async Task<(TaskDto subtask, List<DocTask.Core.Dtos.Period.PeriodDto> periods)?> GetSubtaskWithNearbyPeriods(int parentTaskId, int subtaskId, int secondsTolerance = 5)
    {
        var subtask = await _taskRepository.GetSubtaskById(parentTaskId, subtaskId);
        if (subtask == null)
            return null;

        var dto = TaskMapper.ToDto(subtask);
        var periods = new List<DocTask.Core.Dtos.Period.PeriodDto>();
        if (subtask.PeriodId.HasValue)
        {
            var first = await _periodRepository.GetPeriodById(subtask.PeriodId.Value);
            if (first != null)
            {
                var nearby = await _periodRepository.GetPeriodsNearCreatedAt(first.CreatedAt, secondsTolerance);
                periods = nearby.Select(DocTask.Service.Mappers.PeriodMapper.ToDto).ToList();
            }
        }
        return (dto, periods);
    }

    public async Task<TaskDto> CreateSubtask(int parentTaskId, CreateSubtaskRequest request)
    {
        var subtask = TaskMapper.ToEntity(request);
        subtask.ParentTaskId = parentTaskId;

        await EnsureParentTaskExists(parentTaskId);

        var createdSubtask = await _taskRepository.CreateSubtask(subtask);
        return TaskMapper.ToDto(createdSubtask);
    }

    public async Task<TaskDto> CreateSubtaskWithAssignments(int parentTaskId, CreateSubtaskWithAssignmentsRequest request)
    {
        await EnsureParentTaskExists(parentTaskId);

        // Always create frequency and periods automatically based on provided dates
        int? frequencyId = null;
        List<DocTask.Core.Dtos.Period.PeriodDto> createdPeriods = new();
        if (request.StartDate.HasValue && request.DueDate.HasValue)
        {
            var freqRequest = new CreateFrequencyWithPeriodsRequest
            {
                FrequencyType = request.Frequency?.FrequencyType ?? "weekly",
                FrequencyDetail = null,
                IntervalValue = request.Frequency?.IntervalValue ?? 1,
                StartDate = request.StartDate.Value,
                EndDate = request.DueDate.Value,
                DaysOfWeek = request.Frequency?.DaysOfWeek,
                DayOfMonth = request.Frequency?.DayOfMonth
            };

            var created = await _frequencyService.CreateFrequencyWithPeriods(freqRequest);
            frequencyId = created.Frequency.FrequencyId;
            createdPeriods = created.CreatedPeriods;
        }

        var firstPeriodId = createdPeriods.FirstOrDefault()?.PeriodId;

        var subtask = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            FrequencyId = frequencyId,
            PeriodId = firstPeriodId,
            ParentTaskId = parentTaskId,
            AssignerId = request.AssignerId,
            Status = "pending",
            Priority = "medium",
            Percentagecomplete = 0
        };

        var userIds = request.UserAssignments
            .Select(ua => ua.AssigneeId)
            .ToList();

        var createdSubtask = await _taskRepository.CreateSubtaskWithAssignments(subtask, userIds);
        // Note: this method returns only TaskDto; periods are returned by the other method
        return TaskMapper.ToDto(createdSubtask);
    }

    public async Task<DocTask.Core.Dtos.Tasks.SubtaskCreatedResponse> CreateSubtaskWithAssignmentsAndPeriods(int parentTaskId, CreateSubtaskWithAssignmentsRequest request)
    {
        await EnsureParentTaskExists(parentTaskId);

        List<DocTask.Core.Dtos.Period.PeriodDto> createdPeriods = new();
        int? frequencyId = null;
        if (request.StartDate.HasValue && request.DueDate.HasValue)
        {
            var freqRequest = new CreateFrequencyWithPeriodsRequest
            {
                FrequencyType = request.Frequency?.FrequencyType ?? "weekly",
                FrequencyDetail = null,
                IntervalValue = request.Frequency?.IntervalValue ?? 1,
                StartDate = request.StartDate.Value,
                EndDate = request.DueDate.Value,
                DaysOfWeek = request.Frequency?.DaysOfWeek,
                DayOfMonth = request.Frequency?.DayOfMonth
            };
            var created = await _frequencyService.CreateFrequencyWithPeriods(freqRequest);
            frequencyId = created.Frequency.FrequencyId;
            createdPeriods = created.CreatedPeriods;
        }

        var firstPeriodId2 = createdPeriods.FirstOrDefault()?.PeriodId;

        var userIds = request.UserAssignments.Select(ua => ua.AssigneeId).ToList();
        var subtaskEntity = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            FrequencyId = frequencyId,
            PeriodId = firstPeriodId2,
            ParentTaskId = parentTaskId,
            AssignerId = request.AssignerId,
            Status = "pending",
            Priority = "medium",
            Percentagecomplete = 0
        };

        var createdSubtask = await _taskRepository.CreateSubtaskWithAssignments(subtaskEntity, userIds);
        var dto = TaskMapper.ToDto(createdSubtask);

        return new DocTask.Core.Dtos.Tasks.SubtaskCreatedResponse
        {
            Subtask = dto,
            Periods = createdPeriods
        };
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

    public async Task<TaskDto?> UpdateSubtaskWithAssignments(int parentTaskId, int subtaskId, UpdateSubtaskWithAssignmentsRequest request)
    {
        var existingSubtask = await _taskRepository.GetSubtaskById(parentTaskId, subtaskId);
        if (existingSubtask == null)
            return null;

        if (!string.IsNullOrEmpty(request.Title))
            existingSubtask.Title = request.Title;
        if (request.Description != null)
            existingSubtask.Description = request.Description;
        if (request.StartDate.HasValue)
            existingSubtask.StartDate = request.StartDate;
        if (request.DueDate.HasValue)
            existingSubtask.DueDate = request.DueDate;
        if (request.FrequencyId.HasValue)
            existingSubtask.FrequencyId = request.FrequencyId;
        if (request.PeriodId.HasValue)
            existingSubtask.PeriodId = request.PeriodId;
        if (!string.IsNullOrEmpty(request.AssignerId))
            existingSubtask.AssignerId = request.AssignerId;

        if (request.UserAssignments.Any())
        {
            var userIds = request.UserAssignments
                .Select(ua => ua.AssigneeId)
                .ToList();
            var updatedSubtask = await _taskRepository.UpdateSubtaskWithAssignments(parentTaskId, existingSubtask, userIds);
            return updatedSubtask != null ? TaskMapper.ToDto(updatedSubtask) : null;
        }
        else
        {
            var updatedSubtask = await _taskRepository.UpdateSubtask(parentTaskId, existingSubtask);
            return updatedSubtask != null ? TaskMapper.ToDto(updatedSubtask) : null;
        }
    }

    public async Task<bool> DeleteSubtask(int parentTaskId, int subtaskId)
    {
        return await _taskRepository.DeleteSubtask(parentTaskId, subtaskId);
    }

    public async Task<object> GetDebugData()
    {
        var tasks = await _taskRepository.GetAllTasks();
        var firstTask = tasks.FirstOrDefault();
        
        return new
        {
            TotalTasks = tasks.Count,
            FirstTaskId = firstTask?.TaskId,
            SampleTask = firstTask != null ? new
            {
                TaskId = firstTask.TaskId,
                Title = firstTask.Title,
                FrequencyId = firstTask.FrequencyId,
                PeriodId = firstTask.PeriodId
            } : null,
            Message = "Use this endpoint to check if you have valid parent task IDs, frequency IDs, and period IDs"
        };
    }
}