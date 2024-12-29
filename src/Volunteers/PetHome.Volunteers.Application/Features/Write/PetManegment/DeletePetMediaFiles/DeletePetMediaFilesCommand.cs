using PetHome.Core.Interfaces;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Dto.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesCommand(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto,
    IFilesProvider FileProvider) : ICommand;
