using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.Responses.RefreshToken;

namespace AccountService.Application.Database.Repositories;
public interface IRefreshSessionRepository
{
    public Task Add(RefreshSession refreshSession, CancellationToken ct);

    public Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid id, CancellationToken ct);

    public Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct);

    public Task Remove(RefreshSession refreshSession, CancellationToken ct);

    public Task RemoveOldWithSavingChanges(User user, CancellationToken ct);
}
