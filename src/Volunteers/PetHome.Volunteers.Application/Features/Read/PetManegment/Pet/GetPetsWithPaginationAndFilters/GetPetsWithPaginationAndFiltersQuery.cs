using PetHome.Application.Database.Dtos;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
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
