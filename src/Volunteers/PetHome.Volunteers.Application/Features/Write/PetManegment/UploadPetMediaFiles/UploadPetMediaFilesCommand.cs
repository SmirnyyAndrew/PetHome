using PetHome.Core.Interfaces;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Dto.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.UploadPetMediaFiles;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesDto UploadPetMediaDto,
    Guid VolunteerId,
    IFilesProvider FilesProvider) : ICommand;