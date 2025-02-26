using FilesService.Core.Request.AmazonS3;
using PetHome.Accounts.Application.Features.Write.SetAvatar.UploadPresignedUrlAvatar;

namespace PetHome.Accounts.API.Controllers.Requests;
public record UploadPresignedUrlAvatarRequest(UploadPresignedUrlRequest UploadPresignedUrlRequest)
{
    public static implicit operator UploadPresignedUrlAvatarCommand(UploadPresignedUrlAvatarRequest request)
    {
        return new UploadPresignedUrlAvatarCommand(request.UploadPresignedUrlRequest);
    }
}
