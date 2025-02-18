using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.UploadPresignedUrlAvatar;
public class UploadPresignedUrlAvatarUseCase
    : ICommandHandler<IEnumerable<FileUrlResponse>, UploadPresignedUrlAvatarCommand>
{
    private readonly IAmazonFilesHttpClient _httpClient;

    public UploadPresignedUrlAvatarUseCase(
        IAmazonFilesHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<FileUrlResponse>, ErrorList>> Execute(
        UploadPresignedUrlAvatarCommand command,
        CancellationToken ct)
    {
        List<FileUrlResponse> fileUrlResponses = new List<FileUrlResponse>();

        var uploadPresignedUrlRequests = command.UploadPresignedUrlRequests.ToList();
        for (int i = 0; i < uploadPresignedUrlRequests.Count; i++)
        {
            var uploadPresignedUrlAvatarResult = await _httpClient.UploadPresignedUrl(uploadPresignedUrlRequests[i], ct);
            if (uploadPresignedUrlAvatarResult.IsSuccess)
            {
                FileUrlResponse response = uploadPresignedUrlAvatarResult.Value;
                fileUrlResponses.Add(response);
            }
        }
         
        return fileUrlResponses;
    }
}
