using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Interfaces;

public record DeletePetMediaFilesRequest(
    Guid VolunteerId,
    DeletePetMediaFilesDto DeletePetMediaFilesDto, 
    IFilesProvider FileProvider)
{
    public static implicit operator DeletePetMediaFilesCommand(
        DeletePetMediaFilesRequest request)
    {
        return new DeletePetMediaFilesCommand(
            request.VolunteerId,
            request.DeletePetMediaFilesDto,
            request.FileProvider);
    } 
} 