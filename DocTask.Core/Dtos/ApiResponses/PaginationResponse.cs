namespace DocTask.Core.DTOs.ApiResponses;

public class PaginationResponse<T>
{
    public List<T> Data { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int? PreviousPage => HasPreviousPage ? CurrentPage - 1 : null;
    public int? NextPage => HasNextPage ? CurrentPage + 1 : null;
}
