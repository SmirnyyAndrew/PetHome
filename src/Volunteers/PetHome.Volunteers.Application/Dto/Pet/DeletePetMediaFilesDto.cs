namespace PetHome.Volunteers.Application.Dto.Pet;

public record DeletePetMediaFilesDto(
    Guid PetId,
    string BucketName,
    IEnumerable<string> FilesName);
