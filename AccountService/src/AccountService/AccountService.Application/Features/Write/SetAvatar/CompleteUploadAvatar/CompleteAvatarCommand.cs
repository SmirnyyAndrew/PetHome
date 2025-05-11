using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.SetAvatar.CompleteUploadAvatar;
public record CompleteUploadAvatarCommand(
    Guid UserId,
    string Key,
    CompleteMultipartRequest CompleteMultipartRequest) : ICommand;
