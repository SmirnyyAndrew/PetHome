using System.Data;

namespace PetHome.Application.Database;
public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken ct = default);

    Task SaveChages(CancellationToken ct = default);
}
