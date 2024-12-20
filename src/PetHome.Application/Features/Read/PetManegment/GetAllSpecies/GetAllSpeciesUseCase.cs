using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;

namespace PetHome.Application.Features.Read.PetManegment.GetAllSpecies;
public class GetAllSpeciesUseCase
{
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<GetAllSpeciesUseCase> _logger;

    public GetAllSpeciesUseCase(
        IReadDBContext readDBContext,
        ILogger<GetAllSpeciesUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<SpeciesDto>> Execute(
        CancellationToken ct)
    {
        IReadOnlyList<SpeciesDto> speciesDto =
            await _readDBContext.Species.ToListAsync(ct);
        return speciesDto;
    }
}
