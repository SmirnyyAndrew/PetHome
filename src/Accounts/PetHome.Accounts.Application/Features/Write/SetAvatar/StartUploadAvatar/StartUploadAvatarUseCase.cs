using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using PetHome.Accounts.Application.Features.Write.SetAvatar.StartUploadAvatar;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.SetAvatar.CompleteUploadAvatar;
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
