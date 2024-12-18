using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;

public record UploadPetMediaFilesRequest(
    IEnumerable<Stream> Streams,
    IEnumerable<string> FileNames,
    UploadPetMediaFilesVolunteerDto UploadPetMediaDto)
{
    public static implicit operator UploadPetMediaFilesCommand(
        UploadPetMediaFilesRequest request)
    {
        return new UploadPetMediaFilesCommand(
            request.Streams, 
            request.FileNames, 
            request.UploadPetMediaDto);
    }
} 