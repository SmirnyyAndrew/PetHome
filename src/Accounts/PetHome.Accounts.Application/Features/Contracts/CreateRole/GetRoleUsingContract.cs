using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Application.Features.Contracts.CreateRole;
public class GetRoleUsingContract : IGetRoleContract
{
    private readonly IAuthenticationRepository _repository;

    public GetRoleUsingContract(IAuthenticationRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoleId> Execute(string name, CancellationToken ct)
    {
        RoleName roleName = RoleName.Create(name).Value;
        var roleResult = await _repository.GetRole(roleName);
        RoleId roleId = RoleId.Create(roleResult.Value.Id).Value;

        return roleId;
    }
}
