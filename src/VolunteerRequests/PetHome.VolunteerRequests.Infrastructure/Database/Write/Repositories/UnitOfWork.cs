using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Framework.Database;
using System.Data;

namespace PetHome.VolunteerRequests.Infrastructure.Database.Write.Repositories;
internal class UnitOfWork : IUnitOfWork
{
    private readonly VolunteerRequestDbContext _dbContex;
    public UnitOfWork(VolunteerRequestDbContext dBContext)
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
