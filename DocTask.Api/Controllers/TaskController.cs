using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/task")]
public class TaskController : ControllerBase
{
    
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public IActionResult getAll()
    {
        var result = _taskService.GetAll();

        return Ok(new ApiResponse<List<TaskDto>>
        {
            Data = result,
            Message = "Success",
        });
    }
}