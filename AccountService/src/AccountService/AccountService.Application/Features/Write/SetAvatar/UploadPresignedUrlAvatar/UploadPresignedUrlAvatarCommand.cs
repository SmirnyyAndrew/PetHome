using FilesService.Core.Request.AmazonS3;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.SetAvatar.UploadPresignedUrlAvatar;
public record UploadPresignedUrlAvatarCommand(UploadPresignedUrlRequest UploadPresignedUrlRequest) : ICommand;
