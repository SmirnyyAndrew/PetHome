
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
public record GetAllBreedDtosBySpeciesIdQuery(Guid SpeciesId, int PageNum, int PageSize) : IQuery;
