using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;

namespace DocTask.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly List<TaskModel> _taskList = [
        new TaskModel
        {
            Id = 1,
            Title = "Task1",
        },
        new TaskModel              
        {                          
            Id = 2,                
            Title = "Task2",       
        },                         
        new TaskModel              
        {                          
            Id = 3,                
            Title = "Task3",       
        },                         
        
    ];
    public List<TaskModel> GetAll()
    {
        return _taskList;
    }
}