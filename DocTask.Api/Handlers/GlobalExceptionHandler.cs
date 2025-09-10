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

        int statusCode;
        string message = exception.Message;

        if (exception is BaseException baseException)
        {
            statusCode = baseException.StatusCode;
        }
        else if (exception is UnauthorizedAccessException)
        {
            statusCode = StatusCodes.Status401Unauthorized;
        }
        else if (exception is KeyNotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        else if (exception is ArgumentException || exception is InvalidOperationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }

        httpContext.Response.StatusCode = statusCode;

        var result = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Error = exception.GetType().ToString(),
        };
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}