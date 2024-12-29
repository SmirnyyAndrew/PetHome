using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using PetHome.Core.Controllers;
using PetHome.Core.Response.Validation.Validator;
using PetHome.SharedKernel.Providers.Minio;
using PetHome.Species.API.Controllers.Requests;
using PetHome.Volunteers.API.Controllers.PetManegment.Requests;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;
using PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetStatus;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetHome.Volunteers.Application.Features.Write.PetManegment.DeleteSpeciesById;
using PetHome.Volunteers.Application.Features.Write.PetManegment.HardDelete;
using PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
using PetHome.Volunteers.Application.Features.Write.PetManegment.SoftDeleteRestore;
using PetHome.Volunteers.Application.Features.Write.PetManegment.UploadPetMediaFiles;

namespace PetHome.Volunteers.API.Controllers.PetManegment;

public class PetManegmentController : ParentController
{
    private readonly MinioProvider _minioProvider;

    public PetManegmentController(
        IMinioClient minioClient,
        ILogger<MinioProvider> minioLogger)
    {
        _minioProvider = new MinioProvider(minioClient, minioLogger);
    }


    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<IActionResult> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromBody] PetMainInfoDto PetMainInfoDto,
        [FromServices] CreatePetUseCase createPetUseCase,
        CancellationToken ct = default)
    {
        CreatePetRequest createPetRequest = new CreatePetRequest(
                        volunteerId,
                        PetMainInfoDto);

        var result = await createPetUseCase.Execute(createPetRequest, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("{volunteerId:guid}/pets/media")]
    public async Task<IActionResult> UploadMedia(
        [FromRoute] Guid volunteerId,
        IEnumerable<IFormFile> formFiles,
        [FromQuery] UploadPetMediaFilesDto uploadPetMediaDto,
        [FromServices] UploadPetMediaFilesUseCase uploadPetMediaUseCase,
        CancellationToken ct = default)
    {
        List<Stream> streams = new List<Stream>();
        streams = formFiles.Select(x => x.OpenReadStream()).ToList();
        Result<string, ErrorList> result;


        UploadPetMediaFilesRequest request =
            new UploadPetMediaFilesRequest(
                streams,
                formFiles.ToList().Select(x => x.FileName),
                uploadPetMediaDto,
                volunteerId,
                _minioProvider);

        try
        {
            result = await uploadPetMediaUseCase.Execute(request, ct);
            if (result.IsFailure)
                return BadRequest(result.Error);
        }
        finally
        {
            streams.ForEach(x => x.Dispose());
        }

        return Ok(result.Value);
    }


    [HttpDelete("{volunteerId:guid}/pets/media")]
    public async Task<IActionResult> DeleteMedia(
        [FromRoute] Guid volunteerId,
        [FromBody] DeletePetMediaFilesDto deleteMediaDto,
        [FromServices] DeletePetMediaFilesUseCase deletePetMediaFUseCase,
        CancellationToken ct)
    {
        DeletePetMediaFilesRequest deleteMediaRequest =
            new DeletePetMediaFilesRequest(volunteerId, deleteMediaDto, _minioProvider);

        var deleteResult = await deletePetMediaFUseCase.Execute(
            deleteMediaRequest,
            ct);
        if (deleteResult.IsFailure)
            return BadRequest(deleteResult.Error);

        return Ok(deleteResult.Value);
    }


    [HttpPatch("{volunteerId:guid}/pets/serial-number")]
    public async Task<IActionResult> ChangeSerialNumber(
        [FromRoute] Guid volunteerId,
        [FromBody] ChangePetSerialNumberDto changeNumberDto,
        [FromServices] ChangePetSerialNumberUseCase changeNumberUseCase,
        CancellationToken ct = default)
    {
        ChangePetSerialNumberRequest request =
            new ChangePetSerialNumberRequest(
                volunteerId,
                changeNumberDto);

        var executeResult = await changeNumberUseCase.Execute(request, ct);
        if (executeResult.IsFailure)
            return BadRequest(executeResult.Error);

        return Ok(executeResult.Value);
    }

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

    [HttpPatch("sorted-filtred-paged")]
    public async Task<IActionResult> GetSortedFiltredPagedPets(
        [FromBody] GetPetsWithPaginationAndFiltersRequest request,
        [FromServices] GetPetsWithPaginationAndFiltersUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request,ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }



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
