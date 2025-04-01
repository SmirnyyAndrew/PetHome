using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Dto;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
public record GetPetsWithPaginationAndFiltersQuery(
    Guid? VolunteerId,
    string? Name,
    int? Age,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    Guid? ShelterId,
    double? Weight,
    bool? IsVaccinated,
    bool? IsCastrated,
    PetStatusEnum? Status,
    PagedListDto PagedListDto) : IQuery;
