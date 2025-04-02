using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.Responses.Dto;
using PetManagementService.Domain.PetManagment.PetEntity;

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
