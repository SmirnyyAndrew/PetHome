using PetManagementService.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;

namespace PetManagementService.API.Controllers.Breed.Requests;

public record GetAllBreedDtoBySpeciesIdRequest(int PageNum, int PageSize)
{
    public GetAllBreedDtosBySpeciesIdQuery ToQuery(Guid speciesId)
        => new GetAllBreedDtosBySpeciesIdQuery(speciesId, PageNum, PageSize);
}