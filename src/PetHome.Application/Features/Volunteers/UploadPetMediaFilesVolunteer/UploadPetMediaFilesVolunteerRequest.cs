using PetHome.Application.Features.Dtos;

namespace PetHome.Application.Features.Volunteers.UploadPetMediaFilesVolunteer;
public record UploadPetMediaFilesVolunteerRequest(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto);

public record UploadPetMediaFilesVolunteerDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
