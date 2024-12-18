namespace PetHome.Application.Models;
public class PagedList<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyList<T> Items { get; init; }
    public int Count => Items.Count;
    public bool HasNextPage => PageSize * PageNumber < Count - PageSize; 
    public bool HasPreviousPage => PageNumber > 1;
}
