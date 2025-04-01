using AccountService.Application.Features.Write.SetAvatar.CompleteUploadAvatar;
using FilesService.Core.Request.AmazonS3.MultipartUpload;

namespace AccountService.API.Controllers.Requests.Media;
public record CompleteUploadAvatarRequest(
    Guid UserId,
    string Key,
    CompleteMultipartRequest CompleteMultipartRequest)
{
    public static implicit operator CompleteUploadAvatarCommand(CompleteUploadAvatarRequest request)
    {
        return new CompleteUploadAvatarCommand(request.UserId, request.Key, request.CompleteMultipartRequest);
    }
}