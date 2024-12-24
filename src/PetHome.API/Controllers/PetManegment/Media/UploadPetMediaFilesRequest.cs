using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
using PetHome.Application.Interfaces;

public record UploadPetMediaFilesRequest(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto,
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