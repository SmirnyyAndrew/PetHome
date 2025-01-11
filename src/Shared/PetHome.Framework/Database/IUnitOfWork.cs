using System.Data;

namespace PetHome.Framework.Database;
public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken ct = default);

    Task SaveChanges(CancellationToken ct = default);
}
