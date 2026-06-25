namespace Application.Common.Models;

/// <summary>
/// Pagination request model used for paginated queries.
/// </summary>
/// <param name="Page"></param>
/// <param name="PageSize"></param>
public sealed record PaginationRequest(
    int Page = 1,
    int PageSize = 20);
