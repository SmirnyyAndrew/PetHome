namespace PetHome.Application.Features.Dtos.Pet;

public record UploadPetMediaFilesVolunteerDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
