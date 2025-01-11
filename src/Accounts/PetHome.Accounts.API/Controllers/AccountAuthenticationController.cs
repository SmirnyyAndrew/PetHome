using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests;
using PetHome.Accounts.Application.Features.LoginUser;
using PetHome.Accounts.Application.Features.RegisterAccount;
using PetHome.Accounts.Application.Features.UpdateAccessTokenUsingRefreshToken;
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


    [Authorize]
    [HttpPost("tokens")]
    public async Task<IActionResult> GetNewTokens(
        [FromServices] UpdateAccessTokenUsingRefreshTokenUseCase useCase,
        [FromBody] UpdateAccessTokenUsingRefreshTokenRequest request,
        CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
          
        return Ok(result.Value);
    }
}
