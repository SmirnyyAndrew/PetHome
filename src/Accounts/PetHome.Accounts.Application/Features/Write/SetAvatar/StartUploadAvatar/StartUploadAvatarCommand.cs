using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.SetAvatar.StartUploadAvatar;
public record StartUploadAvatarCommand(StartMultipartUploadRequest StartMultipartUploadRequest) : ICommand;
