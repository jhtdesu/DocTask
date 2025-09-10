using DocTask.Core.DTOs.ApiResponses;

namespace DocTask.Core.DTOs.ApiResponses;

public class PaginatedApiResponse<T> : ApiResponse<PaginationResponse<T>>
{
    public PaginatedApiResponse()
    {
    }

    public PaginatedApiResponse(PaginationResponse<T> data, string? message = null)
    {
        Success = true;
        Data = data;
        Message = message;
    }

    public PaginatedApiResponse(bool success, string error)
    {
        Success = success;
        Error = error;
    }
}
