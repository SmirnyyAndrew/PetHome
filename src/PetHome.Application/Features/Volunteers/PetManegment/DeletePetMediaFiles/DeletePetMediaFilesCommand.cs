using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesCommand(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto);
