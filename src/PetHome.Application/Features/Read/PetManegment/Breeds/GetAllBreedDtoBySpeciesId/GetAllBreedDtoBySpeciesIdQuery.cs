using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Read.PetManegment.Breeds.GetAllBreedDtoBySpeciesId;
public record GetAllBreedDtoBySpeciesIdQuery(Guid SpeciesId) : IQuery;
