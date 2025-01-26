using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.User;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.CreateRole;
public class GetRoleUsingContract : IGetRoleContract
{
    private readonly IAuthenticationRepository _repository;

    public GetRoleUsingContract(IAuthenticationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<RoleId,Error>> Execute(string name, CancellationToken ct)
    {
        RoleName roleName = RoleName.Create(name).Value;
        var roleResult = await _repository.GetRole(roleName);
        if (roleResult.IsFailure)
            return roleResult.Error;

        Role role = roleResult.Value;
        RoleId roleId = RoleId.Create(roleResult.Value.Id).Value;

        return roleId;
    }
}
