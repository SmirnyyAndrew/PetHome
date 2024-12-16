using PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;

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