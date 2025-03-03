using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests.Auth;
using PetHome.Accounts.API.Controllers.Requests.Data;
using PetHome.Accounts.API.Controllers.Requests.EmailManagement;
using PetHome.Accounts.API.Controllers.Requests.Media;
using PetHome.Accounts.Application.Database.Dto;
using PetHome.Accounts.Application.Features.Read.GetUser;
using PetHome.Accounts.Application.Features.Read.GetUserInformation;
using PetHome.Accounts.Application.Features.Write.EmailManagement.ConfirmEmail;
using PetHome.Accounts.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
using PetHome.Accounts.Application.Features.Write.LoginUser;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterUser;
using PetHome.Accounts.Application.Features.Write.SetAvatar.CompleteUploadAvatar;
using PetHome.Accounts.Application.Features.Write.SetAvatar.StartUploadAvatar;
using PetHome.Accounts.Application.Features.Write.SetAvatar.UploadPresignedUrlAvatar;
using PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Accounts.Domain.Constants;
using PetHome.Core.Auth;
using PetHome.Core.Auth.Cookies;
using PetHome.Core.Controllers;
using PetHome.Core.Response.Login;

namespace PetHome.Accounts.API.Controllers;
public class AccountController
    : ParentController
{
    [HttpPost("user/registration")]
    public async Task<IActionResult> Register(
        [FromServices] RegisterUserUseCase useCase,
        [FromBody] RegisterUserRequest request,
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

    [HttpGet("confimation-email/token/{userId:guid}")]
    public async Task<IActionResult> GenerateEmailConfirmation(
       [FromServices] GenerateEmailConfirmationTokenUseCase generateEmailTokenUseCase,
       [FromServices] GetUserInformationUseCase getUserInformationUseCase,
       [FromRoute] Guid userId,
       IPublishEndpoint publisher,
       CancellationToken ct)
    {
        GenerateEmailConfirmationTokenRequest generateEmailTokenRequest =
            new GenerateEmailConfirmationTokenRequest(userId);
        var generateEmailTokenResult =
            await generateEmailTokenUseCase.Execute(generateEmailTokenRequest, ct);

        GetUserInformationRequest getUserInformationRequest =
            new GetUserInformationRequest(userId);
        var getUserInformationResult =
            await getUserInformationUseCase.Execute(getUserInformationRequest, ct);

        if (generateEmailTokenResult.IsFailure)
            return BadRequest(generateEmailTokenResult.Error);

        if (getUserInformationResult.IsFailure)
            return BadRequest(getUserInformationResult.Error);

        ConfirmEmailRequest confirmRequest =
            new ConfirmEmailRequest(userId, generateEmailTokenResult.Value);
        var callbackUrl = Url.Action(
                      nameof(ConfirmEmail),
                      nameof(AccountController).Replace("Controller", string.Empty),
                      confirmRequest,
                      protocol: HttpContext.Request.Scheme);

        UserDto userDto = getUserInformationResult.Value;
        ConfirmedUserEmailEvent createdUserEvent = new ConfirmedUserEmailEvent(
            userDto.Id,
            userDto.Email,
            userDto.UserName,
            callbackUrl);
        await publisher.Publish(createdUserEvent, ct);

        return Ok(callbackUrl);
    }


    [HttpGet("confimation-email")]
    public async Task<IActionResult> ConfirmEmail(
       [FromServices] ConfirmEmailUseCase useCase,
       [FromQuery] ConfirmEmailRequest confirmRequest,
       CancellationToken ct)
    {
        var result = await useCase.Execute(confirmRequest, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Почта подтверждена");
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

    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetUser(
        [FromServices] GetUserUseCase useCase,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        GetUserQuery query = new(id);
        var result = await useCase.Execute(query, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("user-information/{id:guid}")]
    public async Task<IActionResult> GetUserInformation(
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


    [HttpPost("upload-avatar-end")]
    public async Task<IActionResult> CompleteUploadAvatar(
       [FromServices] CompleteUploadAvatarUseCase useCase,
       [FromBody] CompleteUploadAvatarRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("upload-avatar-start")]
    public async Task<IActionResult> StartUploadAvatar(
       [FromServices] StartUploadAvatarUseCase useCase,
       [FromBody] StartUploadAvatarRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("presigned-url-avatar")]
    public async Task<IActionResult> UploadPresignedUrlAvatar(
       [FromServices] UploadPresignedUrlAvatarUseCase useCase,
       [FromBody] UploadPresignedUrlAvatarRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
