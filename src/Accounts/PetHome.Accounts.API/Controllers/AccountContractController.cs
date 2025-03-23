using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.Application.Database.Repositories;

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
        if(getUserResult.IsFailure)
            return new ObjectResult(string.Empty);
         
        return new ObjectResult(getUserResult.Value.Email);
    }
}
