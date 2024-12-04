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
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {

        //throw new ApplicationException("Something went wrong");

        Result<Guid, Error> result = await createUseCase.Execute(request, ct);
         if (result.IsFailure)
             return result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }
}
