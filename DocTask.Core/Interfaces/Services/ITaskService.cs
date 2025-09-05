using DocTask.Core.Dtos.Tasks;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    Task<List<Task>> GetAllTasks();
}