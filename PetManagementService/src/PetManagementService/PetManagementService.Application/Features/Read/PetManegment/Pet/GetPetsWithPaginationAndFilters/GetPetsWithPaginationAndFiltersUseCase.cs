using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
public class GetPetsWithPaginationAndFiltersUseCase
    : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationAndFiltersQuery>
{
    private readonly IPetManagementReadDbContext _readDBContext;

    public GetPetsWithPaginationAndFiltersUseCase(
        IPetManagementReadDbContext readDBContext)
    {
        _readDBContext = readDBContext;
    }


    public async Task<Result<PagedList<PetDto>, ErrorList>> Execute(
        GetPetsWithPaginationAndFiltersQuery query,
        CancellationToken ct)
    {
        var filtredPetsDto = _readDBContext.Pets
        .WhereIf(query.VolunteerId is not null, p => p.VolunteerId == query.VolunteerId)
        .WhereIf(query.Name is not null, p => p.Name.Contains(query.Name))
        .WhereIf(query.Age is not null, p => (DateTime.UtcNow.Year - p.BirthDate.Value.Year) == query.Age)
        .WhereIf(query.SpeciesId is not null, p => p.SpeciesId == query.SpeciesId)
        .WhereIf(query.BreedId is not null, p => p.BreedId == query.BreedId)
        .WhereIf(query.Color is not null, p => p.Color.Contains(query.Color))
        .WhereIf(query.IsVaccinated is not null, p => p.IsVaccinated == query.IsVaccinated)
        .WhereIf(query.IsCastrated is not null, p => p.IsCastrated == query.IsCastrated)
        .WhereIf(query.Status is not null, p => p.Status == query.Status);

        var sortedFiltredPetsDto = filtredPetsDto
            .OrderBy(p => p.Name)
        .OrderBy(p => p.BirthDate.Value)
        .OrderBy(p => p.SpeciesId)
        .OrderBy(p => p.BreedId)
        .OrderBy(p => p.ShelterId)
        .OrderBy(p => p.VolunteerId);

        PagedList<PetDto> pagedSortedFiltredPetsDto = await sortedFiltredPetsDto
            .ToPagedList(query.PagedListDto.PageNum, query.PagedListDto.PageSize, ct);

        return pagedSortedFiltredPetsDto;
    }
}
