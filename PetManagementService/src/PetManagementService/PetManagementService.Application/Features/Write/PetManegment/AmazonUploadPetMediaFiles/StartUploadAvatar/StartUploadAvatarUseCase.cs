using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetManagementService.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.StartUploadAvatar;
public class StartUploadAvatarUseCase
    : ICommandHandler<IEnumerable<UploadPartFileResponse>, StartUploadAvatarCommand>
{
    private readonly IAmazonFilesHttpClient _httpClient;

    public StartUploadAvatarUseCase(
        IAmazonFilesHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<UploadPartFileResponse>, ErrorList>> Execute(
        StartUploadAvatarCommand command,
        CancellationToken ct)
    {
        List<UploadPartFileResponse> uploadResponses = new List<UploadPartFileResponse>();

        var startUploadRequests = command.StartMultipartUploadRequests.ToList();
        for (int i = 0; i < startUploadRequests.Count; i++)
        {
            var startMultipartUploadResult = await _httpClient.StartMultipartUpload(startUploadRequests[i], ct);
            if (startMultipartUploadResult.IsSuccess)
            {
                UploadPartFileResponse response = startMultipartUploadResult.Value;
                uploadResponses.Add(response);
            }
        }
         
        return uploadResponses;
    }
}
