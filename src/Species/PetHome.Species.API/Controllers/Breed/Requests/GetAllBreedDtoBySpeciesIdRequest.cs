using PetHome.Species.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;

namespace PetHome.Species.API.Controllers.Breed.Requests;

public record GetAllBreedDtoBySpeciesIdRequest(Guid SpeciesId)
{
    public static implicit operator GetAllBreedDtoBySpeciesIdQuery(GetAllBreedDtoBySpeciesIdRequest request)
    {
        return new GetAllBreedDtoBySpeciesIdQuery(request.SpeciesId);
    }
}