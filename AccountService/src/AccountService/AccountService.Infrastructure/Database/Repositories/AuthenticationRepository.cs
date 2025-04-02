using AccountService.Application.Database.Repositories;
using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.SharedKernel.Enums;
using PetHome.SharedKernel.Responses.Dto;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;

namespace AccountService.Infrastructure.Database.Repositories;
public class AuthenticationRepository(
    AuthorizationDbContext dbContext,
    UserManager<User> userManager)
    : IAuthenticationRepository
{ 
    public async Task<Result<Role, Error>> GetRole(Guid roleId)
    {
        var result = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);
        if (result is null)
            return Errors.NotFound($"role с id == {roleId}");
        return result;
    }

    public async Task<Result<Role, Error>> GetRole(RoleName roleName)
    {
        var result = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.Value.ToLower());
        if (result is null)
            return Errors.NotFound($"role с name == {roleName}");

        return result;
    }

    public async Task AddUser(User user, CancellationToken ct)
    {
        await dbContext.Users.AddAsync(user, ct);
    }

    public async Task AddUser(IEnumerable<User> users, CancellationToken ct)
    {
        await dbContext.Users.AddRangeAsync(users, ct);
    }

    public async Task AddAdmin(AdminAccount admin, CancellationToken ct)
    {
        await dbContext.Admins.AddAsync(admin, ct);
    }

    public async Task AddVolunteer(VolunteerAccount volunteer, CancellationToken ct)
    {
        await dbContext.VolunteerAccounts.AddAsync(volunteer, ct);
    }

    public async Task AddParticipant(ParticipantAccount participant, CancellationToken ct)
    {
        await dbContext.ParticipantAccounts.AddAsync(participant, ct);
    }

    public void RemoveUser(User user)
    {
        dbContext.Users.Remove(user);
    }

    public void RemoveUser(IEnumerable<User> users)
    {
        dbContext.Users.RemoveRange(users);
    }

    public async Task<IReadOnlyList<User>> GetUsers(CancellationToken ct)
    {
        return await dbContext.Users.ToListAsync(ct);
    }

    public async Task<PagedList<User>> GetPagedUsersWithFilter(
        PagedListDto paginationSettings, UserFilterDto userFilter, CancellationToken ct)
    {
        IQueryable<User> users = dbContext.Users.AsQueryable();

        switch (userFilter.FilterType)
        {
            case UserFilter.NON:
                break;
            case UserFilter.email:
                users = users.Where(u => u.Email.Contains(userFilter.Filter));
                break;
            case UserFilter.username:
                users = users.Where(u => u.UserName.Contains(userFilter.Filter));
                break;
            case UserFilter.role_name:
                users = users.Where(u => u.Role.Name.Contains(userFilter.Filter));
                break;
            case UserFilter.phone_number:
                users = users.Where(u => u.PhoneNumbers.Any(p => p.Value.Contains(userFilter.Filter)));
                break;
            default:
                break;
        }
        var result = await users.ToPagedList(paginationSettings.PageNum, paginationSettings.PageSize, ct);
        return result;
    }

    public async Task<Result<User, Error>> GetUserById(Guid id, CancellationToken ct)
    {
        var users = dbContext.Users.ToList();

        var result = await dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Admin)
            .Include(u => u.Volunteer)
            .Include(u => u.Participant)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (result is null)
            return Errors.NotFound($"user с id == {id}");

        return result;
    }

    public async Task<Result<User, Error>> GetUserByEmail(Email email, CancellationToken ct)
    {
        var result = await dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Admin)
            .Include(u => u.Volunteer)
            .Include(u => u.Participant)
            .FirstOrDefaultAsync(v => v.Email == email);

        if (result is null)
            return Errors.NotFound($"user с email == {email}");

        return result;
    }

    public void UpdateUser(User user, CancellationToken ct)
    {
        dbContext.Users.Update(user);
    }

    public void UpdateUser(IEnumerable<User> users, CancellationToken ct)
    {
        dbContext.Users.UpdateRange(users);
    }

    public async Task<UnitResult<Error>> RemoveById(Guid id, CancellationToken ct)
    {
        var result = await GetUserById(id, ct);
        if (result.IsFailure)
            return result.Error;
        dbContext.Users.Remove(result.Value);

        return UnitResult.Success<Error>();
    }


    public async Task<Result<IReadOnlyList<Permission>, Error>> GetUserPermissions(
        Guid userId, CancellationToken ct)
    {
        var userRoleResult = await GetUserRole(userId, ct);
        if (userRoleResult.IsFailure)
            return userRoleResult.Error;

        Role userRole = userRoleResult.Value;

        var permissionsIds = await dbContext.RolesPermissions
            .Where(r => r.RoleId == userRole.Id)
            .Select(p => p.PermissionId)
            .ToListAsync(ct);
        var permissions = await dbContext.Permissions
            .Where(p => permissionsIds.Contains(p.Id))
            .ToListAsync(ct);
        return permissions;
    }


    public async Task<Result<Role?, Error>> GetUserRole(Guid userId, CancellationToken ct)
    {
        var userResult = await GetUser(userId, ct);
        if (userResult.IsFailure)
            return userResult.Error;

        User user = userResult.Value;
        var userRoleId = user?.RoleId;
        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == userRoleId, ct);
        return role;
    }


    private async Task<Result<User?, Error>> GetUser(Guid id, CancellationToken ct)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (user == null)
            return Errors.NotFound("user");

        return user;
    }
}
