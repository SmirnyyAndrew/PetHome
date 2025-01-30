using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests;
using PetHome.Accounts.Application.Features.Read.GetUserInformation;
using PetHome.Accounts.Application.Features.Write.LoginUser;
using PetHome.Accounts.Application.Features.Write.RegisterAccount;
using PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
using PetHome.Accounts.Domain.Constants;
using PetHome.Core.Auth;
using PetHome.Core.Auth.Cookies;
using PetHome.Core.Controllers;
using PetHome.Core.Response.Dto;
using PetHome.Core.Response.Login;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login(
       [FromServices] LoginUserUseCase useCase,
       [FromBody] LoginUserRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        LoginResponse response = result.Value;
        HttpContext.Response.Cookies.Append(Cookies.RefreshToken.ToString(), response.RefreshToken);

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromServices] UpdateAccessTokenUsingRefreshTokenUseCase useCase,
        CancellationToken ct)
    {
        HttpContext.Request.Cookies.TryGetValue(Cookies.RefreshToken.ToString(), out string? refreshTokenString);
        if (refreshTokenString is null)
            return BadRequest("Refresh token is invalid");

        Guid.TryParse(refreshTokenString, out Guid refreshToken);
        UpdateAccessTokenUsingRefreshTokenRequest request =
            new UpdateAccessTokenUsingRefreshTokenRequest(refreshToken);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure) 
            return BadRequest(result.Error);

        HttpContext.Response.Cookies.Delete(Cookies.RefreshToken.ToString());
        HttpContext.Response.Cookies.Append(Cookies.RefreshToken.ToString(), result.Value.RefreshToken);

        return Ok(result.Value);
    }


    [Permission(Permissions.Pet.GET)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        HttpContext.Response.Cookies.Delete(Cookies.RefreshToken.ToString());
        return Ok();
    }

    [Authorize]
    [HttpPost("auth-checker")]
    public async Task<IActionResult> Test(CancellationToken ct)
    {
        return Ok("It's ok");
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
