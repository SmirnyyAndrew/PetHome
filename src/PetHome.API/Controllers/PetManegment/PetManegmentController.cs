using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Minio;
using PetHome.API.Controllers.PetManegment.Requests;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Application.Features.Write.PetManegment.ChangePetStatus;
using PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Write.PetManegment.HardDelete;
using PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
using PetHome.Application.Features.Write.PetManegment.SoftDeleteRestore;
using PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
using PetHome.Application.Validator;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.API.Controllers.PetManegment;

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
        [FromQuery] UploadPetMediaFilesVolunteerDto uploadPetMediaDto,
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

}
