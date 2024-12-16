namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;

public record DeletePetMediaFilesDto(
    Guid PetId,
    string BucketName,
    IEnumerable<string> FilesName);
