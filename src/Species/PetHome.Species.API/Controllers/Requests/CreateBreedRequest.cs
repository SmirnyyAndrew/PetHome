using PetHome.Application.Features.Write.PetManegment.CreateBreed;

namespace PetHome.API.Controllers.PetManegment.Requests;

public record CreateBreedRequest(Guid SpeciesId, IEnumerable<string> Breeds)
{
    public static implicit operator CreateBreedCommand(CreateBreedRequest requst)
    {
        return new CreateBreedCommand(requst.SpeciesId, requst.Breeds);
    }
}