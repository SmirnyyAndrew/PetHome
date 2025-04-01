using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Enums;
using PetHome.Core.Models;
using PetHome.Core.Response.Dto;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;

namespace AccountService.Application.Database.Repositories;
public interface IAuthenticationRepository
{
    public Task<Result<Role, Error>> GetRole(Guid roleId);

    public Task<Result<Role?, Error>> GetUserRole(Guid userId, CancellationToken ct); 

    public Task<Result<IReadOnlyList<Permission>, Error>> GetUserPermissions(Guid userId, CancellationToken ct);

    public Task<Result<Role, Error>> GetRole(RoleName roleName);

    public Task AddUser(User user, CancellationToken ct);

    public Task AddUser(IEnumerable<User> users, CancellationToken ct);

    public Task AddAdmin(AdminAccount admin, CancellationToken ct);

    public Task AddVolunteer(VolunteerAccount volunteer, CancellationToken ct);

    public Task AddParticipant(ParticipantAccount participant, CancellationToken ct);

    public void RemoveUser(User user);

    public void RemoveUser(IEnumerable<User> users);

    public Task<Result<User, Error>> GetUserById(Guid id, CancellationToken ct);

    public Task<IReadOnlyList<User>> GetUsers(CancellationToken ct);

    public Task<PagedList<User>> GetPagedUsersWithFilter(
        PagedListDto paginationSettings, UserFilterDto userFilter, CancellationToken ct);

    public Task<Result<User, Error>> GetUserByEmail(Email email, CancellationToken ct);

    public void UpdateUser(User user, CancellationToken ct);

    public void UpdateUser(IEnumerable<User> users, CancellationToken ct);

    public Task<UnitResult<Error>> RemoveById(Guid id, CancellationToken ct);
}
