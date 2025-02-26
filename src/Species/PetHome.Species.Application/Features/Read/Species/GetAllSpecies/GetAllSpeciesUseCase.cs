using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Species.Application.Database;
using PetHome.Species.Application.Database.Dto;

namespace PetHome.Species.Application.Features.Read.Species.GetAllSpecies;
public class GetAllSpeciesUseCase
    : IQueryHandler<PagedList<SpeciesDto>, GetAllSpeciesQuery>
{
    private readonly ISpeciesReadDbContext _readDBContext;
    private readonly ILogger<GetAllSpeciesUseCase> _logger;

    public GetAllSpeciesUseCase(
        ISpeciesReadDbContext readDBContext,
        ILogger<GetAllSpeciesUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Execute(
        GetAllSpeciesQuery query,
        CancellationToken ct)
    {  
        var pagedSpeciesDto = await _readDBContext.Species
            .ToPagedList(query.PageNum, query.PageSize, ct);
        return pagedSpeciesDto;
    }
}
