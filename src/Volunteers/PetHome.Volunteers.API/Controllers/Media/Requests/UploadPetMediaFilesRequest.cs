using PetHome.Core.Interfaces;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.UploadPetMediaFiles;

public record UploadPetMediaFilesRequest(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesDto UploadPetMediaDto,
    Guid VolunteerId,
    IFilesProvider FilesProvider)
{
    public static implicit operator UploadPetMediaFilesCommand(
        UploadPetMediaFilesRequest request)
    {
        return new UploadPetMediaFilesCommand(
            request.Streams,
            request.FileNames,
            request.UploadPetMediaDto,
            request.VolunteerId,
            request.FilesProvider);
    }
}