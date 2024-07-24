namespace Api.Core.Extensions;

public class ListPaginations<T>
{
    public List<T> Items { get; }
    public int PageIndex { get; }
    public int TotalPages { get; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public ListPaginations(List<T> items, int pageIndex, int totalPages)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalPages = totalPages;
    }
}
