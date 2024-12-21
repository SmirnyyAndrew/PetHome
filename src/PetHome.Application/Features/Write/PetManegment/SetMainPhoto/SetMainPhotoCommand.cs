namespace PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
public record SetMainPhotoCommand(
     Guid VolunteerId,
     Guid PetId,
     Stream Stream,
     string BucketName,
     string FileName);
