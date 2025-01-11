using Microsoft.EntityFrameworkCore;
using PetHome.Core.Models;
using System.Linq.Expressions;

namespace PetHome.Core.Extentions.Collection;
public static class PagedListExtention
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int pageNum,
        int pageSize,
        CancellationToken ct)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>
        {
            Items = items,
            PageNumber = pageNum,
            PageSize = pageSize
        };
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}
