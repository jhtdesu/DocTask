using DocTask.Core.Dtos.Tasks;
using DocTask.Core.DTOs.ApiResponses;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasks();
    Task<PaginationResponse<TaskDto>> GetTasksPaginated(PaginationRequest request);
    Task<TaskDto?> GetTaskById(int id);
    Task<TaskDto> CreateTask(CreateMainTaskRequest request);
    Task<TaskDto> CreateTaskWithDetails(CreateTaskRequest request);
    Task<TaskDto?> UpdateTask(int id, UpdateTaskRequest request);
    Task<bool> DeleteTask(int id);
    
    // Subtask methods
    Task<List<TaskDto>> GetSubtasksByParentId(int parentTaskId);
    Task<PaginationResponse<TaskDto>> GetSubtasksPaginated(int parentTaskId, PaginationRequest request);
    Task<List<SubtaskWithPeriodsDto>> GetSubtasksWithNearbyPeriods(int parentTaskId, int secondsTolerance = 5);
    Task<PaginationResponse<SubtaskWithPeriodsDto>> GetSubtasksWithNearbyPeriodsPaginated(int parentTaskId, PaginationRequest request, int secondsTolerance = 5);
    Task<TaskDto?> GetSubtaskById(int parentTaskId, int subtaskId);
    Task<(TaskDto subtask, List<DocTask.Core.Dtos.Period.PeriodDto> periods)?> GetSubtaskWithNearbyPeriods(int parentTaskId, int subtaskId, int secondsTolerance = 5);
    Task<TaskDto> CreateSubtask(int parentTaskId, CreateSubtaskRequest request);
    Task<TaskDto> CreateSubtaskWithAssignments(int parentTaskId, CreateSubtaskWithAssignmentsRequest request);
    Task<SubtaskCreatedResponse> CreateSubtaskWithAssignmentsAndPeriods(int parentTaskId, CreateSubtaskWithAssignmentsRequest request);
    Task<TaskDto?> UpdateSubtask(int parentTaskId, int subtaskId, UpdateTaskRequest request);
    Task<TaskDto?> UpdateSubtaskWithAssignments(int parentTaskId, int subtaskId, UpdateSubtaskWithAssignmentsRequest request);
    Task<bool> DeleteSubtask(int parentTaskId, int subtaskId);
    
    // Debug method
    Task<object> GetDebugData();
}