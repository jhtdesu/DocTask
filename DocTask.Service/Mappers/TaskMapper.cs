using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Models;

namespace DocTask.Service.Mappers;

public static class TaskMapper
{
    public static TaskDto ToTaskDto(this TaskModel taskModel)
    {
        return new TaskDto
        {
            Id = taskModel.Id,
            Title = taskModel.Title,
        };
    }
}