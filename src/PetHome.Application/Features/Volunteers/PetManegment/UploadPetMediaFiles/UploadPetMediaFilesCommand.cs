using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFiles;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto);