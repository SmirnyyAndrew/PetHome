using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetHome.API.Controllers.Volunteer.Request;
using PetHome.API.Extentions;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
using PetHome.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.VolunteerEntity;

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

        Result<Guid, ErrorList> result = await createVolunteerUseCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetSatusCode();

        return Ok(result.Value);
    }


    [HttpPatch("{id:guid}/main-info")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoVolunteerDto updateInfoDto,
        [FromServices] UpdateMainInfoVolunteerUseCase updateMainInfoUseCase,
        CancellationToken ct = default)
    {
        UpdateMainInfoVolunteerRequest request = new UpdateMainInfoVolunteerRequest(id, updateInfoDto);

        Result<Guid, ErrorList> result = await updateMainInfoUseCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetSatusCode();

        return Ok(result.Value);
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

        return Ok(result.Value);
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

        return Ok(result.Value);
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

        return Ok(result.Value);
    }


    [HttpDelete("backround")]
    public async Task<IActionResult> HardDeleteSoftDeleted(
        [FromServices] SoftDeletedEntitiesToHardDeleteUseCase useCase,
        CancellationToken ct = default)
    {
        var result = useCase.Execute(ct);
        if (result.IsFailure)
            result.Error.GetSatusCode();

        return Ok(result.Value);
    }


    [HttpGet("paged/volunteers")]
    public async Task<IActionResult> GetAllWithPagination(
        [FromServices] GetAllVolunteersWithPaginationUseCase useCase,
        [FromBody] GetAllVolunteersWithPaginationRequest request,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }



}
