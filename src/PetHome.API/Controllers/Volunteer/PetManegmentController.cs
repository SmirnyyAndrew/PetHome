using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Minio;
using PetHome.API.Extentions;
using PetHome.API.Response;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Volunteers.PetManegment.CreatePetVolunteer;
using PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.API.Controllers.Volunteer;

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
        [FromServices] IValidator<CreatePetRequest> validator,
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
        CancellationToken ct = default)
    {
        List<Stream> streams = new List<Stream>();
        Result<string, Error> result;
        try
        {
            streams = formFiles.Select(x => x.OpenReadStream()).ToList();

            UploadPetMediaFilesRequest uploadPetMediaRequest =
                new UploadPetMediaFilesRequest(
                    streams,
                    formFiles.ToList().Select(x => x.FileName),
                    uploadPetMediaDto);

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
        [FromBody] DeletePetMediaFilesDto deletePetMediaDto,
        [FromServices] DeletePetMediaFilesUseCase deletePetMediaFUseCase,
        CancellationToken ct)
    {
        return default;
    }
}
