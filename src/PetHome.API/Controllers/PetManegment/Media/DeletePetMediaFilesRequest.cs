using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;

public record DeletePetMediaFilesRequest(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto)
{
    public static implicit operator DeletePetMediaFilesCommand(
        DeletePetMediaFilesRequest request)
    {
        return new DeletePetMediaFilesCommand(request
            .VolunteerId,
            request.DeletePetMediaFilesDto);
    } 
} 