namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesRequest(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto);
public record DeletePetMediaFilesDto(
    Guid PetId,
    string BucketName,
    IEnumerable<string> FilesName);
