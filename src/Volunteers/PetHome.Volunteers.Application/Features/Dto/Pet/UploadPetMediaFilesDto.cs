namespace PetHome.Application.Features.Dtos.Pet;

public record UploadPetMediaFilesDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
