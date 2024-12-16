using PetHome.Application.Features.Dtos;

namespace PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto);

public record UploadPetMediaFilesVolunteerDto(
    Guid PetId,
    string BucketName,
    bool CreateBucketIfNotExist);
