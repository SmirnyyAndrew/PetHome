using PetHome.Application.Features.Volunteers.PetManegment.CreateBreed;

namespace PetHome.API.Controllers.PetManegment.Requests;

public record CreateBreedRequst(Guid SpeciesId, IEnumerable<string> Breeds)
{
    public static implicit operator CreateBreedCommand(CreateBreedRequst requst)
    {
        return new CreateBreedCommand(requst.SpeciesId, requst.Breeds);
    }
}