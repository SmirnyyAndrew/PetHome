using PetHome.Species.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
namespace PetHome.Species.API.Controllers.Breed.Requests;

public record GetAllBreedDtoBySpeciesIdRequest(int PageNum, int PageSize)
{
    public GetAllBreedDtosBySpeciesIdQuery ToQuery(Guid speciesId)
        => new GetAllBreedDtosBySpeciesIdQuery(speciesId, PageNum, PageSize);
}