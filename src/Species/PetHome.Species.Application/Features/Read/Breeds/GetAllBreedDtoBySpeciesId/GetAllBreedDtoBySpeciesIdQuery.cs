
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Species.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
public record GetAllBreedDtoBySpeciesIdQuery(Guid SpeciesId) : IQuery;
