namespace PetManagementService.Application.Dto.Pet;

public record UploadPetMediaFilesDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
