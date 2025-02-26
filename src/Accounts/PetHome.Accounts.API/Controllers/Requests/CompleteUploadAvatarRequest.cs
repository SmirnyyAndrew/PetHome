using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Accounts.Application.Features.Write.SetAvatar.CompleteUploadAvatar;

namespace PetHome.Accounts.API.Controllers.Requests;
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