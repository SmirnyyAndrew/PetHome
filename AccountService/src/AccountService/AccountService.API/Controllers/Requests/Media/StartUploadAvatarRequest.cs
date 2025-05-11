using AccountService.Application.Features.Write.SetAvatar.StartUploadAvatar;
using FilesService.Core.Request.AmazonS3.MultipartUpload;

namespace AccountService.API.Controllers.Requests.Media;
public record StartUploadAvatarRequest(StartMultipartUploadRequest StartMultipartUploadRequest)
{
    public static implicit operator StartUploadAvatarCommand(StartUploadAvatarRequest request)
    {
        return new StartUploadAvatarCommand(request.StartMultipartUploadRequest);
    }
}
