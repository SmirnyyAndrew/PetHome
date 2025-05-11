using System.Data;

namespace PetHome.Core.Infrastructure.Database;
public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken ct = default);

    Task SaveChanges(CancellationToken ct = default);
}
