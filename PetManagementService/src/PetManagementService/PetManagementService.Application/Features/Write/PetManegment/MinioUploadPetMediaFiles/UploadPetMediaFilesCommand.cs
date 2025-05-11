using FilesService.Core.Interfaces;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Dto.Pet;

namespace PetManagementService.Application.Features.Write.PetManegment.MinioUploadPetMediaFiles;
public record UploadPetMediaFilesCommand(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesDto UploadPetMediaDto,
    IMinioFilesHttpClient FilesHttpClient,
    Guid VolunteerId) : ICommand;