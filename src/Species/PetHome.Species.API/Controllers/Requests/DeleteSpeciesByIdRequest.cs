using PetHome.Species.Application.Features.Write.DeleteSpeciesById;

namespace PetHome.Species.API.Controllers.Requests;
 
public record DeleteSpeciesByIdRequest(Guid SpeciesId)
{
    public static implicit operator DeleteSpeciesByIdCommand(DeleteSpeciesByIdRequest request)
    {
        return new DeleteSpeciesByIdCommand(request.SpeciesId);
    }
}
