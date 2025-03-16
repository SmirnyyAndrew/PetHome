using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _cache;

    public GetAllSpeciesUseCase(
        ISpeciesReadDbContext readDBContext,
        ILogger<GetAllSpeciesUseCase> logger,
        IMemoryCache cache)
    {
        _readDBContext = readDBContext;
        _logger = logger;
        _cache = cache;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Execute(
        GetAllSpeciesQuery query,
        CancellationToken ct)
    {
        string key = "species";

        var cachedSpecieses = _cache.Get<IEnumerable<SpeciesDto>>(key);
        if (cachedSpecieses is null)
        {
            PagedList<SpeciesDto> pagedSpeciesDto = await _readDBContext.Species
               .ToPagedList(query.PageNum, query.PageSize, ct);

            if (pagedSpeciesDto.Count > 0)
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };
                _cache.Set(key, pagedSpeciesDto.Items, cacheOptions);
            }

            return pagedSpeciesDto;
        }

        var cachedPagedSpeciesDto = cachedSpecieses
            .ToPagedList(query.PageNum, query.PageSize);

        return cachedPagedSpeciesDto;
    }
}
