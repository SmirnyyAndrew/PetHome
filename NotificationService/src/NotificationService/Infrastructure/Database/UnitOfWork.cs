using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace NotificationService.Infrastructure.Database;

public class UnitOfWork(NotificationDbContext dbContext)
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken ct)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(ct);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}
