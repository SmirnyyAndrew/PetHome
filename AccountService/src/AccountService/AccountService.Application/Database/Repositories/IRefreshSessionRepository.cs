using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.RefreshToken;

namespace AccountService.Application.Database.Repositories;
public interface IRefreshSessionRepository
{
    public Task Add(RefreshSession refreshSession, CancellationToken ct);

    public Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid id, CancellationToken ct);

    public Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct);

    public Task Remove(RefreshSession refreshSession, CancellationToken ct);

    public Task RemoveOldWithSavingChanges(User user, CancellationToken ct);
}
