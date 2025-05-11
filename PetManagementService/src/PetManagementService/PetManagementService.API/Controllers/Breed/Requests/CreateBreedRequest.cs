using PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;

namespace PetManagementService.API.Controllers.Breed.Requests;

public record CreateBreedRequest(Guid SpeciesId, IEnumerable<string> Breeds)
{
    public static implicit operator CreateBreedCommand(CreateBreedRequest requst)
    {
        return new CreateBreedCommand(requst.SpeciesId, requst.Breeds);
    }
}