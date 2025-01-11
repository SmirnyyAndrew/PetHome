namespace PetHome.Volunteers.Application.Features.Dto.Pet;

public record DeletePetMediaFilesDto(
    Guid PetId,
    string BucketName,
    IEnumerable<string> FilesName);
