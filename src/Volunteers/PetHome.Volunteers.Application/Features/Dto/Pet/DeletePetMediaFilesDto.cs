namespace PetHome.Application.Features.Dtos.Pet;

public record DeletePetMediaFilesDto(
    Guid PetId,
    string BucketName,
    IEnumerable<string> FilesName);
