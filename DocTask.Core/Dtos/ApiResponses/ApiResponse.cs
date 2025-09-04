namespace DocTask.Core.DTOs.ApiResponses;

public class ApiResponse <T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
}