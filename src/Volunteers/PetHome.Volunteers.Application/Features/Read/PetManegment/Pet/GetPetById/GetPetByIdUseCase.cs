using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.AmazonS3;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;
public class GetPetByIdUseCase
    : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly ILogger<GetPetByIdUseCase> _logger;
    private readonly IAmazonFilesHttpClient _httpClient;

    public GetPetByIdUseCase(
        IVolunteerReadDbContext readDBContext,
        IAmazonFilesHttpClient httpClient,
        ILogger<GetPetByIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Result<PetDto, ErrorList>> Execute(
        GetPetByIdQuery query,
        CancellationToken ct)
    {
        var petDto = _readDBContext.Pets.FirstOrDefault(p => p.Id == query.PetId);
        if (petDto == null)
        {
            _logger.LogError("Питомец с id = {0} не существует", query.PetId);
            return Errors.NotFound($"Питомец с id = {query.PetId}").ToErrorList();
        }

        MediaFile? avatar = petDto.Avatar;
        GetPresignedUrlRequest getPresignedAvatarUrlRequest = new GetPresignedUrlRequest(avatar.BucketName);
        var getAvatarUrlResult = await _httpClient.GetPresignedUrl(avatar.Key.ToString(), getPresignedAvatarUrlRequest, ct);
        if (getAvatarUrlResult.IsSuccess)
        {
            petDto.AvatarUrl = getAvatarUrlResult.Value.Url;
        }
         
        List<string> photosUrls = new List<string>();
        foreach (var photo in petDto.Photos)
        {
            GetPresignedUrlRequest getPresignedPhotoUrlRequest = new GetPresignedUrlRequest(avatar.BucketName);
            var getPhotoUrlResult = await _httpClient.GetPresignedUrl(photo.Key.ToString(), getPresignedPhotoUrlRequest, ct);
            if(getPhotoUrlResult.IsSuccess)
                photosUrls.Add(getPhotoUrlResult.Value.Url);
        }
        petDto.PhotosUrls = photosUrls;

        return petDto;
    }
}
