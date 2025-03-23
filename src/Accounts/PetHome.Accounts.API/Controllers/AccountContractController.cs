using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Application.Features.Contracts.AuthManagement.GetRole;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateRefreshToken;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetPermissionsNames;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetRolesNames;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetUserMainInformation;
using PetHome.Accounts.Contracts.HttpCommunication.Requests.TokenManagement;
using PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement;
using PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
using System.Text.Json;

namespace PetHome.Accounts.API.Controllers;

[ApiController]
[Route("contract")]
public class AccountContractController
{
    [HttpGet("user-by-email/{userId:guid}")]
    public async Task<IActionResult> GetUserByEmail(
       IAuthenticationRepository repository,
       [FromRoute] Guid userId,
       CancellationToken ct)
    {
        var getUserResult = await repository.GetUserById(userId, ct);
        if (getUserResult.IsFailure)
            return new ObjectResult(string.Empty);

        return new ObjectResult(getUserResult.Value.Email);
    }


    [HttpGet("role-id-by-name/{name}")]
    public async Task<IActionResult> GetRoleIdByName(
        [FromRoute] string name,
        [FromServices] GetRoleIdByNameUseCase useCase,
        CancellationToken ct)
    {
        GetRoleIdByNameQuery query = new(name);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure)
            return new ObjectResult(result.Error);

        return new ObjectResult(result.Value);
    }


    [HttpPost("access-token")]
    public async Task<IActionResult> GenerateAccessToken(
        [FromBody] Guid userId,
        [FromServices] GenerateAccessTokenUseCase useCase,
        CancellationToken ct)
    {
        GenerateAccessTokenCommand command = new(userId);
        var result = await useCase.Execute(command, ct);
        if (result.IsFailure)
            return new ObjectResult(string.Empty);

        return new ObjectResult(result.Value);
    }


    //[HttpPost("refresh-token")]
    //public async Task<IActionResult> GenerateRefreshToken(
    //    [FromBody] Guid userId,
    //    [FromBody] string accessToken,
    //    [FromServices] GenerateRefreshTokenUseCase useCase,
    //    CancellationToken ct)
    //{
    //    GenerateRefreshTokenCommand command = new(userId, accessToken);
    //    var result = await useCase.Execute(command, ct);
    //    if (result.IsFailure)
    //        return new ObjectResult(string.Empty);

    //    return new ObjectResult(result.Value);
    //}


    [HttpPost("user-permissions-codes/{userId:guid}")]
    public async Task<IActionResult> GetUserPermissionsCodes(
        [FromRoute] Guid userId,
        [FromServices] GetUserPermissionsCodesUseCase useCase,
        CancellationToken ct)
    {
        GetUserPermissionsCodesQuery query = new(userId);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure)
            return new ObjectResult(result.Error);

        return new ObjectResult(result.Value);
    }


    [HttpGet("user-role-name/{userId:guid}")]
    public async Task<IActionResult> GetUserRoleName(
        [FromRoute] Guid userId,
        [FromServices] GetUserRoleNameUseCase useCase,
        CancellationToken ct)
    {
        GetUserRoleNameQuery query = new(userId);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure)
            return new ObjectResult(result.Error);

        return new ObjectResult(result.Value);
    }


    [HttpGet("user-main-information/{userId:guid}")]
    public async Task<IActionResult> GetUserMainInformation(
        [FromRoute] Guid userId,
        [FromServices] GetUserMainInformationUseCase useCase,
        CancellationToken ct)
    {
        GetUserMainInformationQuery query = new(userId);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure)
            return new ObjectResult(result.Error);

        return new ObjectResult(result.Value);
    }
}
