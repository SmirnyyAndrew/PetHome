using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.StartUploadAvatar;
public record StartUploadAvatarCommand(
    IEnumerable<StartMultipartUploadRequest> StartMultipartUploadRequests) : ICommand;
