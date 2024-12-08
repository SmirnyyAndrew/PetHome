using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetHome.API.Extentions;
using PetHome.API.Response;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Volunteers.CreateVolunteer;
using PetHome.Application.Features.Volunteers.HardDeleteVolunteer;
using PetHome.Application.Features.Volunteers.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Volunteers.SoftDeleteVolunteer;
using PetHome.Application.Features.Volunteers.UpdateMainInfoVolunteer;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Controllers.Volunteer;
public class VolunteerDataManegmentController : ParentController
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


    [HttpDelete("hard/{id:guid}")]
    public async Task<IActionResult> HardDelete(
         [FromRoute] Guid id,
         [FromServices] HardDeleteVolunteerUseCase hardDeleteUseCase,
         CancellationToken ct)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;

        var result = await hardDeleteUseCase.Execute(volunteerId, ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }


    [HttpPatch("soft/{id:guid}")]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerUseCase softDeleteVoUseCase,
        CancellationToken ct = default)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;

        var result = await softDeleteVoUseCase.Execute(id, ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }


    [HttpPatch("soft-re/{id:guid}")]
    public async Task<IActionResult> SoftRestore(
        [FromRoute] Guid id,
        [FromServices] SoftRestoreVolunteerUseCase softRestoreUseCase,
        CancellationToken ct = default)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;

        var result = await softRestoreUseCase.Execute(id, ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }


    [HttpDelete("backround")]
    public async Task<IActionResult> HardDeleteSoftDeleted(
        [FromServices] SoftDeletedEntitiesToHardDeleteUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(ResponseEnvelope.Ok(result.Value));
    }
}
