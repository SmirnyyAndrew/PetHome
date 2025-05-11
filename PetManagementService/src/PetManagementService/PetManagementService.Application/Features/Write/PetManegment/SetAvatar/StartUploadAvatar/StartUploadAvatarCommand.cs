using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.StartUploadAvatar;
public record StartUploadAvatarCommand(StartMultipartUploadRequest StartMultipartUploadRequest) :ICommand;
