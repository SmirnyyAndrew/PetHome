using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.RolePermission;

namespace AccountService.Application.Features.Contracts.AuthManagement.GetRole;
public class GetRoleIdByNameUseCase
    : IQueryHandler<Guid?, GetRoleIdByNameQuery>
{
    private readonly IAuthenticationRepository _repository;

    public GetRoleIdByNameUseCase(IAuthenticationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid?, ErrorList>> Execute(GetRoleIdByNameQuery query, CancellationToken ct)
    {
        RoleName roleName = RoleName.Create(query.Name).Value;
        var roleResult = await _repository.GetRole(roleName);
        if (roleResult.IsFailure)
            return Guid.Empty;

        Role role = roleResult.Value;
        RoleId roleId = RoleId.Create(roleResult.Value.Id).Value;

        return roleId.Value;
    } 
}
