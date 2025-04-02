using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.SetMainPhoto;
public record SetPetMainPhotoCommand(
     Guid VolunteerId,
     Guid PetId,
     string BucketName,
     string FileName) : ICommand;
