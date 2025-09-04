using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DockTask.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "AN ERROR OCCURRED: {Message}", exception.Message);
        httpContext.Response.ContentType = "application/json";
        
        if (exception is BaseException baseException)
        {
            httpContext.Response.StatusCode = baseException.StatusCode;
        }
        else
        {
            httpContext.Response.StatusCode = 500;
        }
        
        var result = new ApiResponse<object>
        {
            Success = false,
            Message = exception.Message,
            Error = exception.GetType().ToString(),
        };
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}