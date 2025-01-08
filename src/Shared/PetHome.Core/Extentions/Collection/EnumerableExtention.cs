using Microsoft.EntityFrameworkCore;
using PetHome.Core.Interfaces.Database;

namespace PetHome.Core.Extentions.Collection;
public static class EnumerableExtention
{
    public static List<T> GetExpiredEntitiesList<T>(this IEnumerable<T> list, int daysToHardDelete) where T : ISoftDeletableEntity
    {
        return list
            .Where(s => s.DeletionDate.AddDays(daysToHardDelete) >= DateTime.UtcNow)
            .ToList();
    }
}
