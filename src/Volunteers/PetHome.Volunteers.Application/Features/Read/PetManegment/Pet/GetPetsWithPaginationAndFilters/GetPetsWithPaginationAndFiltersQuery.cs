using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
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
