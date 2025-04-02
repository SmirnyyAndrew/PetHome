using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.StartUploadAvatar;
public record StartUploadAvatarCommand(
    IEnumerable<StartMultipartUploadRequest> StartMultipartUploadRequests) : ICommand;
