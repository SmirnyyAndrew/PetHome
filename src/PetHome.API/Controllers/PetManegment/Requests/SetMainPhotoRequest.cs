using PetHome.Application.Features.Write.PetManegment.SetMainPhoto;

namespace PetHome.API.Controllers.PetManegment.Requests;
public record SetMainPhotoRequest(
     Guid VolunteerId,
     Guid PetId,
     Stream Stream,
     string BucketName,
     string FileName)
{
    public static implicit operator SetMainPhotoCommand(SetMainPhotoRequest request)
    {
        return new SetMainPhotoCommand(
            request.VolunteerId,
            request.PetId,
            request.Stream,
            request.BucketName,
            request.FileName);
    }
}
