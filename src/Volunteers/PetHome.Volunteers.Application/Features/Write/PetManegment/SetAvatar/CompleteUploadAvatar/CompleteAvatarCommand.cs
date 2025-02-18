using FilesService.Core.Request.AmazonS3.MultipartUpload;
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetAvatar.CompleteUploadAvatar;
public record CompleteUploadAvatarCommand(
    Guid VolunteerId,
    Guid PetId, 
    string Key,
    CompleteMultipartRequest CompleteMultipartRequest) : ICommand;
