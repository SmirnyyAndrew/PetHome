using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.UploadPresignedUrlAvatar;
public class UploadPresignedUrlAvatarUseCase
    : ICommandHandler<FileUrlResponse, UploadPresignedUrlAvatarCommand>
{
    private readonly IAmazonFilesHttpClient _httpClient;

    public UploadPresignedUrlAvatarUseCase(
        IAmazonFilesHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<FileUrlResponse, ErrorList>> Execute(
        UploadPresignedUrlAvatarCommand command,
        CancellationToken ct)
    {
        var uploadPresignedUrlAvatarResult = await _httpClient.UploadPresignedUrl(command.UploadPresignedUrlRequest, ct);
        if (uploadPresignedUrlAvatarResult.IsFailure)
            return Errors.Failure(uploadPresignedUrlAvatarResult.Error).ToErrorList();

        FileUrlResponse result = uploadPresignedUrlAvatarResult.Value;
        return result;
    }
}
