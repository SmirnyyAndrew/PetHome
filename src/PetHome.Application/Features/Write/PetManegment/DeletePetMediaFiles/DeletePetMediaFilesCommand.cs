using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Interfaces.FeatureManagment; 

namespace PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesCommand(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto) : ICommand;
