using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.StartUploadAvatar;
public class StartUploadAvatarUseCase
    : ICommandHandler<UploadPartFileResponse, StartUploadAvatarCommand>
{
    private readonly IAmazonFilesHttpClient _httpClient;

    public StartUploadAvatarUseCase(
        IAmazonFilesHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<UploadPartFileResponse, ErrorList>> Execute(
        StartUploadAvatarCommand command,
        CancellationToken ct)
    {
        var startMultipartUploadResult = await _httpClient.StartMultipartUpload(command.StartMultipartUploadRequest, ct);
        if (startMultipartUploadResult.IsFailure)
            return Errors.Failure(startMultipartUploadResult.Error).ToErrorList();

        UploadPartFileResponse result = startMultipartUploadResult.Value;
        return result;
    }
}
