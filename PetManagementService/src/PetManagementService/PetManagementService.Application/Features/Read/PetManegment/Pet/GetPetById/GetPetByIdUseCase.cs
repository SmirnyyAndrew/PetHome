using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.AmazonS3;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetById;
public class GetPetByIdUseCase
    : IQueryHandler<PetDto?, GetPetByIdQuery>
{
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ILogger<GetPetByIdUseCase> _logger;
    private readonly IAmazonFilesHttpClient _httpClient;
    private readonly ICacheService _cacheService;

    public GetPetByIdUseCase(
        IPetManagementReadDbContext readDBContext,
        IAmazonFilesHttpClient httpClient,
        ILogger<GetPetByIdUseCase> logger,
        ICacheService cacheService)
    {
        _readDBContext = readDBContext;
        _logger = logger;
        _httpClient = httpClient;
        _cacheService = cacheService;
    }

    public async Task<Result<PetDto?, ErrorList>> Execute(
        GetPetByIdQuery query,
        CancellationToken ct)
    {
        PetDto? cachedPetDto = await _cacheService.GetOrSetAsync<PetDto>(Constants.Redis.PET(query.PetId), async () =>
        {
            var petDto = _readDBContext.Pets.FirstOrDefault(p => p.Id == query.PetId);
            if (petDto is null)
            {
                _logger.LogError("Питомец с id = {0} не существует", query.PetId);
                return null;
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
                if (getPhotoUrlResult.IsSuccess)
                    photosUrls.Add(getPhotoUrlResult.Value.Url);
            }
            petDto.PhotosUrls = photosUrls;

            return petDto;
        }, options: null, ct);
         
        return cachedPetDto;
    }
}
