using FilesService.Core.Request.AmazonS3;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.UploadPartPresignedUrlAvatar;
public record UploadPresignedUrlAvatarCommand(
    IEnumerable<UploadPresignedUrlRequest> UploadPresignedUrlRequests) : ICommand;
