using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Minio;
using PetHome.API.Envelopes;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Volunteers.PetManegment.ChangeSerialNumber;
using PetHome.Application.Features.Volunteers.PetManegment.CreatePet;
using PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFiles;
using PetHome.Domain.Shared.Error;
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
        [FromServices] IValidator<CreatePetCommand> validator,
        CancellationToken ct = default)
    {
        CreatePetRequest createPetRequest = new CreatePetRequest(
                        volunteerId,
                        PetMainInfoDto);

        var validationResult = await validator.ValidateAsync(createPetRequest, ct);
        if (validationResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validationResult.Errors));

        var result = await createPetUseCase.Execute(createPetRequest, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(ResponseEnvelope.Ok(result.Value));
    }


    [HttpPost("{volunteerId:guid}/pets/media")]
    public async Task<IActionResult> UploadMedia(
        [FromRoute] Guid volunteerId,
        IEnumerable<IFormFile> formFiles,
        [FromQuery] UploadPetMediaFilesVolunteerDto uploadPetMediaDto,
        [FromServices] UploadPetMediaFilesUseCase uploadPetMediaUseCase,
        [FromServices] IValidator<UploadPetMediaFilesCommand> validator,
        CancellationToken ct = default)
    {
        List<Stream> streams = new List<Stream>();
        streams = formFiles.Select(x => x.OpenReadStream()).ToList();
        Result<string, Error> result;

        UploadPetMediaFilesRequest uploadPetMediaRequest =
            new UploadPetMediaFilesRequest(
                streams,
                formFiles.ToList().Select(x => x.FileName),
                uploadPetMediaDto);

        var validationResult = await validator.ValidateAsync(
            uploadPetMediaRequest,
            ct);

        if (validationResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validationResult.Errors));

        try
        {
            result = await uploadPetMediaUseCase.Execute(
              _minioProvider,
              uploadPetMediaRequest,
              volunteerId,
              ct);
            if (result.IsFailure)
                return BadRequest(ResponseEnvelope.Error(result.Error));
        }
        finally
        {
            streams.ForEach(x => x.Dispose());
        }

        return Ok(ResponseEnvelope.Ok(result.Value));
    }


    [HttpDelete("{volunteerId:guid}/pets/media")]
    public async Task<IActionResult> DeleteMedia(
        [FromRoute] Guid volunteerId,
        [FromBody] DeletePetMediaFilesDto deleteMediaDto,
        [FromServices] DeletePetMediaFilesUseCase deletePetMediaFUseCase,
        [FromServices] IValidator<DeletePetMediaFilesCommand> validator,
        CancellationToken ct)
    {
        DeletePetMediaFilesRequest deleteMediaRequest =
            new DeletePetMediaFilesRequest(volunteerId, deleteMediaDto);

        var validationResult = await validator.ValidateAsync(deleteMediaRequest, ct);
        if (validationResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validationResult.Errors));

        var deleteResult = await deletePetMediaFUseCase.Execute(
            _minioProvider,
            deleteMediaRequest,
            ct);
        if (deleteResult.IsFailure)
            return BadRequest(ResponseEnvelope.Error(deleteResult.Error)); ;

        return Ok(ResponseEnvelope.Ok(deleteResult.Value));
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
            return BadRequest(ResponseEnvelope.Error(executeResult.Error));

        return Ok(ResponseEnvelope.Ok(executeResult.Value));
    }
}
