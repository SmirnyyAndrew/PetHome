using PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;

namespace PetHome.Volunteers.API.Controllers.PetManagment.Requests;
public record SetPetMainPhotoRequest(
     Guid VolunteerId,
     Guid PetId, 
     string BucketName,
     string FileName)
{
    public static implicit operator SetPetMainPhotoCommand(SetPetMainPhotoRequest request)
    {
        return new SetPetMainPhotoCommand(
            request.VolunteerId,
            request.PetId, 
            request.BucketName,
            request.FileName);
    }
}
