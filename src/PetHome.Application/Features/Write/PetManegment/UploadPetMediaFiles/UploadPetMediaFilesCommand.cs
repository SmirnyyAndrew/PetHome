using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto,
    Guid VolunteerId,
    IFilesProvider FilesProvider) : ICommand;