using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
public record SetPetMainPhotoCommand(
     Guid VolunteerId,
     Guid PetId,
     string BucketName,
     string FileName) : ICommand;
