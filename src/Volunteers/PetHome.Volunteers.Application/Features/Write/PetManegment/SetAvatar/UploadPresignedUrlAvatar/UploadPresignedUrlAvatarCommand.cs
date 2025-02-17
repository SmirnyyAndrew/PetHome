using FilesService.Core.Request.AmazonS3;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetAvatar.UploadPresignedUrlAvatar;
public record UploadPresignedUrlAvatarCommand(UploadPresignedUrlRequest UploadPresignedUrlRequest) : ICommand;
