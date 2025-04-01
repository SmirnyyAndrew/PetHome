using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Framework.Database;
using System.Data;

namespace DiscussionService.Infrastructure.Database.Write.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly DiscussionDbContext _dbContex;
    public UnitOfWork(DiscussionDbContext dBContext)
    {
        _dbContex = dBContext;
    }
    public async Task<IDbTransaction> BeginTransaction(CancellationToken ct = default)
    {
        var transaction = await _dbContex.Database.BeginTransactionAsync(ct);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken ct = default)
    {
        await _dbContex.SaveChangesAsync(ct);
    }
}
