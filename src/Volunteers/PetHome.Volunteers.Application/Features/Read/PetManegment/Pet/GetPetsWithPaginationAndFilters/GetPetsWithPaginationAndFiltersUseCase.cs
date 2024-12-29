using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetsWithPaginationAndFilters;
public class GetPetsWithPaginationAndFiltersUseCase
    : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationAndFiltersQuery>
{
    private readonly IReadDBContext _readDBContext; 

    public GetPetsWithPaginationAndFiltersUseCase(
        IReadDBContext readDBContext)
    {
        _readDBContext = readDBContext; 
    }


    public async Task<Result<PagedList<PetDto>, ErrorList>> Execute(
        GetPetsWithPaginationAndFiltersQuery query,
        CancellationToken ct)
    {
        var filtredPetsDto = _readDBContext.Pets
             .WhereIf(query.VolunteerId is not null, p => p.VolunteerId == query.VolunteerId)
             .WhereIf(query.Name is not null, p => p.Name == query.Name)
             .WhereIf(query.Age is not null, p => (DateTime.UtcNow.Year - p.BirthDate.Value.Year) == query.Age)
             .WhereIf(query.SpeciesId is not null, p => p.SpeciesId == query.SpeciesId)
             .WhereIf(query.BreedId is not null, p => p.BreedId == query.BreedId)
             .WhereIf(query.Color is not null, p => p.Color == query.Color)
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

        var pagedSortedFiltredPetsDto = await sortedFiltredPetsDto
            .ToPagedList(query.PagedListDto.PageNum, query.PagedListDto.PageSize, ct);

        return pagedSortedFiltredPetsDto;
    }
}
