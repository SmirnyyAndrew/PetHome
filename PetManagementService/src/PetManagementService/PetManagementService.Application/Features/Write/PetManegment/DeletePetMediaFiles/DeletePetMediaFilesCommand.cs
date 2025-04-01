using FilesService.Core.Interfaces;
using PetHome.Core.Interfaces.FeatureManagment;
using PetManagementService.Application.Dto.Pet;

namespace PetManagementService.Application.Features.Write.PetManegment.DeletePetMediaFiles;
public record DeletePetMediaFilesCommand(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto,
    IMinioFilesHttpClient FileProvider) : ICommand;
