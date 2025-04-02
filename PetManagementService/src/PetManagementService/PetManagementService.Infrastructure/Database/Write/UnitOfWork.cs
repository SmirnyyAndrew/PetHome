using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Core.Infrastructure.Database;
using PetManagementService.Infrastructure.Database.Write.DBContext;
using System.Data;

namespace PetManagementService.Infrastructure.Database.Write;
public class UnitOfWork(PetManagementWriteDBContext dbContex)
    : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken ct = default)
    {
        var transaction = await dbContex.Database.BeginTransactionAsync(ct);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken ct = default)
    {
        await dbContex.SaveChangesAsync(ct);
    }
}
