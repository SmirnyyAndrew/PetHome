using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PetHome.Core.Interfaces;

namespace PetHome.SharedKernel.Interceptors;
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker
            .Entries<SoftDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.SoftDelete();
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
