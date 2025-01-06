using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Framework.Database;
using System.Data;

namespace PetHome.Accounts.Infrastructure.Database.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AuthorizationDbContext _dbContex;
    public UnitOfWork(AuthorizationDbContext dBContext)
    {
        _dbContex = dBContext;
    }
    public async Task<IDbTransaction> BeginTransaction(CancellationToken ct = default)
    {
        var transaction = await _dbContex.Database.BeginTransactionAsync(ct);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChages(CancellationToken ct = default)
    {
        await _dbContex.SaveChangesAsync(ct);
    }
}
