using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests;
using PetHome.Accounts.Application.Features.LoginAccount;
using PetHome.Accounts.Application.Features.RegisterAccount;
using PetHome.Core.Auth;
using PetHome.Core.Controllers;

namespace PetHome.Accounts.API.Controllers;
public class AccountAuthenticationController : ParentController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromServices] RegisterAccountUseCase useCase,
        [FromBody] RegisterAccountRequest request,
        CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpPatch("login")]
    public async Task<IActionResult> Login(
       [FromServices] LoginAccountUseCase useCase,
       [FromBody] LoginAccountRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [Permission("get.pet")] 
    [HttpGet("permission-test")]
    public async Task<IActionResult> TestPermission()
    { 
        return Ok("Everything is fine!");
    }
}
