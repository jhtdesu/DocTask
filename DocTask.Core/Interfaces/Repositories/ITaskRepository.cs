using DocTask.Core.Models;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<List<Task>> GetAllTasks();
}