namespace PetHome.Volunteers.Application.Dto.Pet;

public record UploadPetMediaFilesDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
