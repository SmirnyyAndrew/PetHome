using System.Collections;

namespace PetHome.Application.Models;
public class PagedList<T> : IReadOnlyList<T>
{ 
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyList<T> Items { get; init; }
    public int Count => Items.Count;
    public bool HasNextPage => PageSize * PageNumber < Count - PageSize;
    public bool HasPreviousPage => PageNumber > 1;

    public T this[int index] => Items[index];
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
