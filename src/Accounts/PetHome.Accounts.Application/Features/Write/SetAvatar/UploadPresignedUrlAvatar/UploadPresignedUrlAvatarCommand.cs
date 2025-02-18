using FilesService.Core.Request.AmazonS3;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.SetAvatar.UploadPresignedUrlAvatar;
public record UploadPresignedUrlAvatarCommand(UploadPresignedUrlRequest UploadPresignedUrlRequest) : ICommand;
