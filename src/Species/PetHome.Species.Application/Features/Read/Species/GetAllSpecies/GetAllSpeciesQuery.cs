using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Species.Application.Features.Read.Species.GetAllSpecies;
public record GetAllSpeciesQuery(int PageNum, int PageSize) : IQuery;
