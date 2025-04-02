
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
public record GetAllBreedDtosBySpeciesIdQuery(Guid SpeciesId, int PageNum, int PageSize) : IQuery;
