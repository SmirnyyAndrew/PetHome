using FilesService.Core.Interfaces;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Dto.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesCommand(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto,
    IMinioFilesHttpClient FileProvider) : ICommand;
