using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetHome.API.Extentions;
using PetHome.API.Response;
using PetHome.Application.Features.Volunteers.CreateVolunteer;
using PetHome.Application.Features.Volunteers.HardDeleteVolunteer;
using PetHome.Application.Features.Volunteers.UpdateMainInfoVolunteer;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Controllers;
public class VolunteerController : ParentController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerUseCase createVolunteerUseCase,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {

        //throw new ApplicationException("Something went wrong");

        Result<Guid, Error> result = await createVolunteerUseCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoVolunteerDto updateInfoDto,
        [FromServices] UpdateMainInfoVolunteerUseCase updateMainInfoUseCase,
        [FromServices] IValidator<UpdateMainInfoVolunteerRequest> validator,
        CancellationToken ct = default)
    {
        UpdateMainInfoVolunteerRequest request = new UpdateMainInfoVolunteerRequest(id, updateInfoDto);
        ValidationResult validationResult = await validator.ValidateAsync(request, ct);

        if (validationResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validationResult.Errors));

        Result<Guid, Error> result = await updateMainInfoUseCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerUseCase hardDeleteUseCase,
        [FromServices] ILogger<HardDeleteVolunteerUseCase> logger,
        CancellationToken ct)
    {
        VolunteerId volunteerId = VolunteerId.Create(id);

        var result = await hardDeleteUseCase.Execute(volunteerId, ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }
}
