using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.StartUploadAvatar;
public record StartUploadAvatarCommand(StartMultipartUploadRequest StartMultipartUploadRequest) :ICommand;
