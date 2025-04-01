using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Read.Species.GetAllSpecies;
public record GetAllSpeciesQuery(int PageNum, int PageSize) : IQuery;
