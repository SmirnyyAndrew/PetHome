using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.API.Controllers.Requests;
using PetHome.Accounts.Application.Features.RegisterAccount;
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

        return Ok(result);
    }
}
