using PetHome.Application.Features.Write.PetManegment.DeleteSpeciesById;

namespace PetHome.API.Controllers.PetManegment.Requests;
 
public record DeleteSpeciesByIdRequest(Guid SpeciesId)
{
    public static implicit operator DeleteSpeciesByIdCommand(DeleteSpeciesByIdRequest request)
    {
        return new DeleteSpeciesByIdCommand(request.SpeciesId);
    }
}
