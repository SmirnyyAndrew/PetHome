using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Volunteers.API.Controllers.Volunteer.Request;
using PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
using PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetVolunteerById;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeletedEntitiesToHardDelete;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
using PetHome.Volunteers.Contracts;

namespace PetHome.Volunteers.API.Controllers.Volunteer;
public class VolunteerController : ParentController
{ 
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICreateVolunteerContract useCase,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {

        //throw new ApplicationException("Something went wrong");

        Result<Guid, ErrorList> result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [Authorize]
    [HttpPatch("{id:guid}/main-info")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoVolunteerDto updateInfo,
        [FromServices] UpdateMainInfoVolunteerUseCase useCase,
        CancellationToken ct = default)
    {
        UpdateMainInfoVolunteerRequest request = new UpdateMainInfoVolunteerRequest(id, updateInfo);

        Result<Guid, ErrorList> result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [Authorize]
    [HttpDelete("hard/{id:guid}")]
    public async Task<IActionResult> HardDelete(
         [FromRoute] Guid id,
         [FromServices] HardDeleteVolunteerUseCase useCase,
         CancellationToken ct)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;
        HardDeleteVolunteerRequest request = new HardDeleteVolunteerRequest(volunteerId);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [Authorize]
    [HttpPatch("soft/{id:guid}")]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerUseCase useCase,
        CancellationToken ct = default)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;
        SoftDeleteRestoreVolunteerRequest request = new SoftDeleteRestoreVolunteerRequest(volunteerId);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [Authorize]
    [HttpPatch("soft-re/{id:guid}")]
    public async Task<IActionResult> SoftRestore(
        [FromRoute] Guid id,
        [FromServices] SoftRestoreVolunteerUseCase useCase,
        CancellationToken ct = default)
    {
        VolunteerId volunteerId = VolunteerId.Create(id).Value;
        SoftDeleteRestoreVolunteerRequest request = new SoftDeleteRestoreVolunteerRequest(volunteerId);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [Authorize]
    [HttpDelete("backround")]
    public async Task<IActionResult> HardDeleteSoftDeleted(
        [FromServices] SoftDeletedEntitiesToHardDeleteUseCase useCase,
        CancellationToken ct = default)
    {
        var result = useCase.Execute(ct);
        if (result.IsFailure)
            result.Error.GetStatusCode();

        return Ok(result.Value);
    }


    [HttpGet("paged/volunteers")]
    public async Task<IActionResult> GetAllWithPagination(
        [FromServices] GetAllVolunteersWithPaginationUseCase useCase,
        [FromQuery] GetAllVolunteersWithPaginationRequest request,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpGet("volunteer/{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromServices] GetVolunteerByIdUseCase useCase,
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        GetVolunteerByIdRequest request = new GetVolunteerByIdRequest(id);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
