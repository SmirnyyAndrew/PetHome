using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Dtos;
using PetHome.Application.Database.Read;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

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

    public async Task<Result<IReadOnlyList<BreedDto>, ErrorList>> Execute(
        Guid speciesId,
        CancellationToken ct)
    {
        SpeciesDto? getSpeciesByIdResult = await _readDBContext.Species
            .Include(b=>b.Breeds)
            .Where(s => s.Id == speciesId)
            .FirstOrDefaultAsync();
        if (getSpeciesByIdResult == null)
        {
            _logger.LogError("Вид животного с id - {0} не найден", speciesId);
            return (ErrorList)Errors.NotFound($"вид животного с id - {speciesId}");
        }

        var breedDtos = getSpeciesByIdResult.Breeds.ToList();
        return breedDtos;
    }
}
