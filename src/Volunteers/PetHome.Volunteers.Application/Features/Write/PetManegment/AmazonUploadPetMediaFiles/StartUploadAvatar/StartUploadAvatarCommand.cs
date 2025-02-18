using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.StartUploadAvatar;
public record StartUploadAvatarCommand(
    IEnumerable<StartMultipartUploadRequest> StartMultipartUploadRequests) : ICommand;
