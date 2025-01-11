using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
public record SetPetMainPhotoCommand(
     Guid VolunteerId,
     Guid PetId,
     string BucketName,
     string FileName) : ICommand;
