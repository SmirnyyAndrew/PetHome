using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
        [FromServices] CreateVolunteerUseCase createUseCase,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {
        var validatorResult = await validator.ValidateAsync(request, ct);

        if (validatorResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validatorResult));

        Result<Guid, Error> result = await createUseCase.Execute(request, ct);
         if (result.IsFailure)
             return result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }
}
