namespace PetHome.Volunteers.Application.Features.Dto.Pet;

public record UploadPetMediaFilesDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
