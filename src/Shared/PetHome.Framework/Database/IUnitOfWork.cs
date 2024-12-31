using System.Data;

namespace PetHome.Framework.Database;
public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken ct = default);

    Task SaveChages(CancellationToken ct = default);
}
