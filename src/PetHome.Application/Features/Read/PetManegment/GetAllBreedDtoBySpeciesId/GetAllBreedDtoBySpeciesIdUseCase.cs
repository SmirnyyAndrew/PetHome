using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Dtos;
using PetHome.Application.Database.Read;

namespace PetHome.Application.Features.Read.PetManegment.GetAllBreedDtoBySpeciesId;
public class GetAllBreedDtoBySpeciesIdUseCase
{
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<GetAllBreedDtoBySpeciesIdUseCase> _logger;

    public GetAllBreedDtoBySpeciesIdUseCase(
        IReadDBContext readDBContext,
        ILogger<GetAllBreedDtoBySpeciesIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<BreedDto> Execute(
        Guid speciesId,
        CancellationToken ct)
    {
        return default;
    }
}
