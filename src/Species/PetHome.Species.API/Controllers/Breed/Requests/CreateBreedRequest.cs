using PetHome.Species.Application.Features.Write.CreateBreed;

namespace PetHome.Species.API.Controllers.Breed.Requests;

public record CreateBreedRequest(Guid SpeciesId, IEnumerable<string> Breeds)
{
    public static implicit operator CreateBreedCommand(CreateBreedRequest requst)
    {
        return new CreateBreedCommand(requst.SpeciesId, requst.Breeds);
    }
}