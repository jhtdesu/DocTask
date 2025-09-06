namespace DocTask.Core.Dtos.Tasks;

public class TaskDto
{
    public int TaskId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AssignerId { get; set; }
    public string? AssigneeId { get; set; }
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
}

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AssignerId { get; set; } 
    public string? AssigneeId { get; set; }
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
}