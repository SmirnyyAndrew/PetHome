using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Infrastructure.Database.Repositories;
public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AuthorizationDbContext _dbContext;
    public AuthenticationRepository(AuthorizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Role, Error>> GetRole(Guid roleId)
    {
        var result = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (result is null)
            return Errors.NotFound($"role с id == {roleId}");
        return result;
    }

    public async Task<Result<Role, Error>> GetRole(RoleName roleName)
    {
        var result = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.Value.ToLower());
        if (result is null)
            return Errors.NotFound($"role с name == {roleName}");

        return result;
    }

    public async Task AddUser(User user, CancellationToken ct)
    {
        await _dbContext.Users.AddAsync(user, ct);
    }

    public async Task AddUser(IEnumerable<User> users, CancellationToken ct)
    {
        await _dbContext.Users.AddRangeAsync(users, ct);
    }

    public async Task AddAdmin(AdminAccount admin, CancellationToken ct)
    {
        await _dbContext.Admins.AddAsync(admin, ct);
    }

    public async Task AddVolunteer(VolunteerAccount volunteer, CancellationToken ct)
    {
        await _dbContext.VolunteerAccounts.AddAsync(volunteer, ct);
    }

    public async Task AddParticipant(ParticipantAccount participant, CancellationToken ct)
    {
        await _dbContext.ParticipantAccounts.AddAsync(participant, ct);
    }

    public void RemoveUser(User user)
    {
        _dbContext.Users.Remove(user);
    }

    public void RemoveUser(IEnumerable<User> users)
    {
        _dbContext.Users.RemoveRange(users);
    }

    public async Task<Result<User, Error>> GetUserById(Guid id, CancellationToken ct)
    {
        var result = await _dbContext.Users
            .FirstOrDefaultAsync(v => v.Id == id);

        if (result is null)
            return Errors.NotFound($"user с id == {id}");

        return result;
    }

    public async Task<Result<User, Error>> GetUserByEmail(Email email, CancellationToken ct)
    {
        var result = await _dbContext.Users
            .FirstOrDefaultAsync(v => v.Email == email);

        if (result is null)
            return Errors.NotFound($"user с email == {email}");

        return result;
    }

    public void UpdateUser(User user, CancellationToken ct)
    {
        _dbContext.Users.Update(user);
    }

    public void UpdateUser(IEnumerable<User> users, CancellationToken ct)
    {
        _dbContext.Users.UpdateRange(users);
    }

    public async Task<UnitResult<Error>> RemoveById(Guid id, CancellationToken ct)
    {
        var result = await GetUserById(id, ct);
        if (result.IsFailure)
            return result.Error;
        _dbContext.Users.Remove(result.Value);

        return UnitResult.Success<Error>();
    }
}
