using DocTask.Core.Dtos.Tasks;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    List<TaskDto> GetAll();
}