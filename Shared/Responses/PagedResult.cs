namespace test.Shared.Responses;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalRecords { get; init; }

    public int TotalPages { get; init; }

    public PagedResult(
        IReadOnlyCollection<T> items,
        int page,
        int pageSize,
        int totalRecords)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
}