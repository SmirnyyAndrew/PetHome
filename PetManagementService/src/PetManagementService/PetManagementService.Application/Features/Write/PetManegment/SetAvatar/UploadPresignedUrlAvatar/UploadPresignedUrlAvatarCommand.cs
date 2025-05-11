using FilesService.Core.Request.AmazonS3;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.UploadPresignedUrlAvatar;
public record UploadPresignedUrlAvatarCommand(UploadPresignedUrlRequest UploadPresignedUrlRequest) : ICommand;
