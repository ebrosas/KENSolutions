namespace KenHRApp.Application.Common.Models;

/// <summary>Server-side paging payload consumed by MudDataGrid's ServerData callback.</summary>
public sealed class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public static PagedResult<T> Empty(int pageSize) => new(Array.Empty<T>(), 0, 1, pageSize);
}
