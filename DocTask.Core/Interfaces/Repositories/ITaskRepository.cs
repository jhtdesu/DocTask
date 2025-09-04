using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    List<TaskModel> GetAll();
}