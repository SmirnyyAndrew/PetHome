using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto);