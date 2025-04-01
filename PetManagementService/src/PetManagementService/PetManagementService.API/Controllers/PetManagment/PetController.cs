using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetManagementService.API.Controllers.PetManagment.Requests;
using PetManagementService.Application.Dto.Pet;
using PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetById;
using PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetInfo;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;
using PetManagementService.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetManagementService.Application.Features.Write.PetManegment.CreatePet;
using PetManagementService.Application.Features.Write.PetManegment.DeleteSpeciesById;
using PetManagementService.Application.Features.Write.PetManegment.HardDelete;
using PetManagementService.Application.Features.Write.PetManegment.SetMainPhoto;
using PetManagementService.Application.Features.Write.PetManegment.SoftDeleteRestore;

namespace PetManagementService.API.Controllers.PetManagment;

public class PetController : ParentController
{   
    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<IActionResult> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromBody] PetMainInfoDto petMainInfo,
        [FromServices] CreatePetUseCase useCase,
        CancellationToken ct = default)
    {
        CreatePetRequest createPetRequest = new CreatePetRequest(
                        volunteerId,
                        petMainInfo);

        var result = await useCase.Execute(createPetRequest, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

     

    [Authorize]
    [HttpPatch("{volunteerId:guid}/pets/serial-number")]
    public async Task<IActionResult> ChangeSerialNumber(
        [FromRoute] Guid volunteerId,
        [FromBody] ChangePetSerialNumberDto changeNumberDto,
        [FromServices] ChangePetSerialNumberUseCase useCase,
        CancellationToken ct = default)
    {
        ChangePetSerialNumberRequest request =
            new ChangePetSerialNumberRequest(
                volunteerId,
                changeNumberDto);

        var executeResult = await useCase.Execute(request, ct);
        if (executeResult.IsFailure)
            return BadRequest(executeResult.Error);

        return Ok(executeResult.Value);
    }


    [Authorize]
    [HttpPost("info")]
    public async Task<IActionResult> ChangeInfo(
        [FromBody] ChangePetInfoRequest request,
        [FromServices] ChangePetInfoUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [Authorize]
    [HttpPost("status")]
    public async Task<IActionResult> ChangeStatus(
        [FromBody] ChangePetStatusRequest request,
        [FromServices] ChangePetStatusUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [Authorize]
    [HttpDelete("hard")]
    public async Task<IActionResult> HardDelete(
        [FromBody] HardDeletePetRequest request,
        [FromServices] HardDeletePetUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [Authorize]
    [HttpPost("soft")]
    public async Task<IActionResult> HardDelete(
        [FromBody] SoftDeleteRestorePetRequest request,
        [FromServices] SoftDeleteRestorePetUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("main-photo")]
    public async Task<IActionResult> SetMainPhoto(
        [FromBody] SetPetMainPhotoRequest request,
        [FromServices] SetPetMainPhotoUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPetById(
        [FromRoute] Guid id,
        [FromServices] GetPetByIdUseCase useCase,
        CancellationToken ct = default)
    {
        GetPetByIdRequest request = new GetPetByIdRequest(id);
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("sorted-filtred-paged")]
    public async Task<IActionResult> GetSortedFilteredPagedPets(
        [FromQuery] GetPetsWithPaginationAndFiltersRequest request,
        [FromServices] GetPetsWithPaginationAndFiltersUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request,ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpeciesWithBreeds(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesByIdUseCase useCase,
        CancellationToken ct)
    {
        DeleteSpeciesByIdRequest request = new DeleteSpeciesByIdRequest(id);
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
