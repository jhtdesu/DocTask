using DocTask.Core.Enums;
using DocTask.Core.Dtos.Frequency;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DocTask.Core.Dtos.Period;

namespace DocTask.Core.Dtos.Tasks;

public class TaskDto
{
    public int TaskId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AssignerId { get; set; }
    public int? OrgId { get; set; }
    public int? PeriodId { get; set; }
    public int? AttachedFile { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? UnitId { get; set; }
    public int? FrequencyId { get; set; }
    public int? Percentagecomplete { get; set; }
    public int? ParentTaskId { get; set; }
    public DateTime? FrequencyDate { get; set; }
    public List<UserAssignmentDto> UserAssignments { get; set; } = new List<UserAssignmentDto>();
}

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AssignerId { get; set; } 
    public int? OrgId { get; set; }
    public int? PeriodId { get; set; }
    public int? AttachedFile { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? UnitId { get; set; }
    public int? FrequencyId { get; set; }
    public int? Percentagecomplete { get; set; }
    public int? ParentTaskId { get; set; }
}

public class CreateMainTaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
}

public class CreateSubtaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? FrequencyId { get; set; }
    public int? PeriodId { get; set; }
}

public class UserAssignmentDto
{
    public string AssigneeId { get; set; } = null!;
}

public class CreateSubtaskWithAssignmentsRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssignerId { get; set; }
    public List<UserAssignmentDto> UserAssignments { get; set; } = new List<UserAssignmentDto>();
    public InlineFrequencyRequest? Frequency { get; set; }
}

public class InlineFrequencyRequest
{
    public string FrequencyType { get; set; } = null!; // daily, weekly, monthly, quarterly, yearly
    [DefaultValue(1)]
    [Range(1, int.MaxValue)]
    public int IntervalValue { get; set; } = 1;
    [DefaultValue(new int[] { })]
    public List<int>? DaysOfWeek { get; set; } // 0=Sunday..6=Saturday
    [Range(1, 31)]
    public int? DayOfMonth { get; set; } // 1..31
}

public class SubtaskCreatedResponse
{
    public TaskDto Subtask { get; set; } = null!;
    public List<DocTask.Core.Dtos.Period.PeriodDto> Periods { get; set; } = new List<DocTask.Core.Dtos.Period.PeriodDto>();
}

public class SubtaskWithPeriodsDto
{
    public TaskDto Subtask { get; set; } = null!;
    public List<PeriodDto> Periods { get; set; } = new List<PeriodDto>();
}

public class UpdateSubtaskWithAssignmentsRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? FrequencyId { get; set; }
    public int? PeriodId { get; set; }
    public string? AssignerId { get; set; }
    public List<UserAssignmentDto> UserAssignments { get; set; } = new List<UserAssignmentDto>();
}