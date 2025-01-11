using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests;
using PetHome.Accounts.Application.Features.Read.GetUserInformation;
using PetHome.Accounts.Application.Features.Write.LoginUser;
using PetHome.Accounts.Application.Features.Write.RegisterAccount;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Constants;
using PetHome.Core.Auth;
using PetHome.Core.Controllers;

namespace PetHome.Accounts.API.Controllers;
public class AccountAuthenticationController : ParentController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromServices] RegisterParticipantUserUseCase useCase,
        [FromBody] RegisterParticipantUserRequest request,
        CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpPatch("login")]
    public async Task<IActionResult> Login(
       [FromServices] LoginUserUseCase useCase,
       [FromBody] LoginUserRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    //Указал Permission-заглушку для доступа этого метода у Participant
    [Permission(Permissions.Pet.GET)]
    [HttpGet("user-information/{id:guid}")]
    public async Task<IActionResult> GetUserInformationWithHisAccounts(
        [FromServices] GetUserInformationUseCase useCase,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        GetUserInformationQuery query = new(id);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure) 
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
