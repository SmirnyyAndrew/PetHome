using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Accounts.Domain.Tokens.RefreshToken;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Application.Database.Repositories;
public interface IRefreshSessionRepository
{
    public Task<Result<RefreshSession, Error>> GetById(Guid id, CancellationToken ct);

    public Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct);

    public Task<UnitResult<Error>> Remove(RefreshSession refreshSession, CancellationToken ct);
}
