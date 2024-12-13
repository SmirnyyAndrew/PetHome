using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Application.Database;
using System.Data;

namespace PetHome.Infrastructure.DataBase;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _dbContex;
    public UnitOfWork(ApplicationDBContext dBContext)
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
