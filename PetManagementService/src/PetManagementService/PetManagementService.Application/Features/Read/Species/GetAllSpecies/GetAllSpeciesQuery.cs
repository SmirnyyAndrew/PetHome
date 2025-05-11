using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Read.Species.GetAllSpecies;
public record GetAllSpeciesQuery(int PageNum, int PageSize) : IQuery;
