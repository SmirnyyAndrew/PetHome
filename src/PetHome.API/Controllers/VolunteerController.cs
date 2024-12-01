using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using PetHome.API.Extentions;
using PetHome.API.Response;
using PetHome.Application.Volunteers.CreateVolunteer;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerUseCase useCase,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {
        Result<Guid,Error> result = await useCase.Execute(request, ct);

        if (result.IsFailure) 
            return result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value)); 
    }
}
