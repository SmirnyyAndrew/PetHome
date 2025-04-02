using Microsoft.EntityFrameworkCore;
using PetHome.Core.Application.Interfaces.Database;

namespace PetHome.Core.Web.Extentions.Collection;
public static class EnumerableExtention
{
    public static List<T> GetExpiredEntitiesList<T>(this IEnumerable<T> list, int daysToHardDelete) where T : ISoftDeletableEntity
    {
        return list
            .Where(s => s.IsDeleted == true
                && s.DeletionDate != default
                && s.DeletionDate.AddDays(daysToHardDelete) < DateTime.UtcNow)
            .ToList();
    }
}
