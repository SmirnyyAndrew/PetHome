using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Aggregates.User.Accounts;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects;

namespace PetHome.Accounts.Application.Database.Repositories;
public interface IAuthenticationRepository
{
    public Task<Result<Role, Error>> GetRole(Guid roleId);

    public Task<Result<Role, Error>> GetRole(RoleName roleName);

    public Task AddUser(User user, CancellationToken ct);

    public Task AddUser(IEnumerable<User> users, CancellationToken ct);

    public Task AddAdmin(AdminAccount admin, CancellationToken ct);

    public Task AddVolunteer(VolunteerAccount volunteer, CancellationToken ct);

    public Task AddParticipant(ParticipantAccount participant, CancellationToken ct);

    public void RemoveUser(User user);

    public void RemoveUser(IEnumerable<User> users);

    public Task<Result<User, Error>> GetUserById(Guid id, CancellationToken ct);

    public Task<Result<User, Error>> GetUserByEmail(Email email, CancellationToken ct);

    public void UpdateUser(User user, CancellationToken ct);

    public void UpdateUser(IEnumerable<User> users, CancellationToken ct);

    public Task<UnitResult<Error>> RemoveById(Guid id, CancellationToken ct);
}
