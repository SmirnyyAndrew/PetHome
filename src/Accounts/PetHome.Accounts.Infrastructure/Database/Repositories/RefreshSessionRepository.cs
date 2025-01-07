using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Tokens.RefreshToken;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Infrastructure.Database.Repositories;
public class RefreshSessionRepository(AuthorizationDbContext dbContext) 
    : IRefreshSessionRepository
{
    public async Task<Result<RefreshSession, Error>> GetById(
        Guid id, CancellationToken ct)
    {
        var result = await dbContext.RefreshSessions
             .FirstOrDefaultAsync(r => r.Id == id, ct);
        
        if(result is null)
            return Errors.NotFound($"Refresh session с id = {id}");

        return result;
    }

    public async Task<UnitResult<Error>> Remove(
        Guid id, CancellationToken ct)
    {
        var result = await dbContext.RefreshSessions
             .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (result is null)
            return Errors.NotFound($"Refresh session с id = {id}");

        dbContext.RefreshSessions.Remove(result);
        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> Remove(
        RefreshSession refreshSession, CancellationToken ct)
    {
        dbContext.RefreshSessions.Remove(refreshSession);
        return UnitResult.Success<Error>();
    }
}
