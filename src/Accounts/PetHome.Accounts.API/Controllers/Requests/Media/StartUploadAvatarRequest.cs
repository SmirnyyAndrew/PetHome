using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Accounts.Application.Features.Write.SetAvatar.StartUploadAvatar;

namespace PetHome.Accounts.API.Controllers.Requests.Media;
public record StartUploadAvatarRequest(StartMultipartUploadRequest StartMultipartUploadRequest)
{
    public static implicit operator StartUploadAvatarCommand(StartUploadAvatarRequest request)
    {
        return new StartUploadAvatarCommand(request.StartMultipartUploadRequest);
    }
}
