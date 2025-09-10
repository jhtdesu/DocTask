namespace DocTask.Core.Enums;

public enum TaskAssignmentType
{
    /// <summary>
    /// User is the primary assignee responsible for completing the task
    /// </summary>
    Assignee = 1,
    
    /// <summary>
    /// User is assigned to collaborate on the task
    /// </summary>
    Collaborator = 2,
    
    /// <summary>
    /// User is assigned to review or approve the task
    /// </summary>
    Reviewer = 3,
    
    /// <summary>
    /// User is assigned to monitor or track the task progress
    /// </summary>
    Observer = 4
}
