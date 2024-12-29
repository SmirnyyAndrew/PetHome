using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Framework.Database;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using System.Data;

namespace PetHome.Species.Infrastructure.Database.Write;
public class UnitOfWork : IUnitOfWork
{
    private readonly SpeciesWriteDBContext _dbContex;
    public UnitOfWork(SpeciesWriteDBContext dBContext)
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
