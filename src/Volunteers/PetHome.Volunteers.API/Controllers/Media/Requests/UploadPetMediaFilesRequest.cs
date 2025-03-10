using FilesService.Core.Interfaces;
using PetHome.Volunteers.Application.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.MinioUploadPetMediaFiles;

public record UploadPetMediaFilesRequest(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesDto UploadPetMediaDto,
    Guid VolunteerId,
    IMinioFilesHttpClient FilesHttpClient)
{
    public static implicit operator UploadPetMediaFilesCommand(
        UploadPetMediaFilesRequest request)
    {
        return new UploadPetMediaFilesCommand(
            request.Streams,
            request.FileNames,
            request.UploadPetMediaDto,
            request.FilesHttpClient,
            request.VolunteerId);
    }
}