using AccountService.Application.Features.Write.SetAvatar.UploadPresignedUrlAvatar;
using FilesService.Core.Request.AmazonS3;

namespace AccountService.API.Controllers.Requests.Media;
public record UploadPresignedUrlAvatarRequest(UploadPresignedUrlRequest UploadPresignedUrlRequest)
{
    public static implicit operator UploadPresignedUrlAvatarCommand(UploadPresignedUrlAvatarRequest request)
    {
        return new UploadPresignedUrlAvatarCommand(request.UploadPresignedUrlRequest);
    }
}
