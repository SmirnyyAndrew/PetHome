using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.SetAvatar.StartUploadAvatar;
public record StartUploadAvatarCommand(StartMultipartUploadRequest StartMultipartUploadRequest) : ICommand;
