using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Species.Application.Database;
using PetHome.Species.Application.Database.Dto;

namespace PetHome.Species.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
public class GetAllBreedDtoBySpeciesIdUseCase
    : IQueryHandler<IReadOnlyList<BreedDto>, GetAllBreedDtoBySpeciesIdQuery>
{
    private readonly  ISpeciesReadDbContext _readDBContext;
    private readonly ILogger<GetAllBreedDtoBySpeciesIdUseCase> _logger;

    public GetAllBreedDtoBySpeciesIdUseCase(
        ISpeciesReadDbContext readDBContext,
        ILogger<GetAllBreedDtoBySpeciesIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<BreedDto>, ErrorList>> Execute(
        GetAllBreedDtoBySpeciesIdQuery query,
        CancellationToken ct)
    {
        SpeciesDto? getSpeciesByIdResult = await _readDBContext.Species
            .Include(b => b.Breeds)
            .Where(s => s.Id == query.SpeciesId)
            .FirstOrDefaultAsync();
        if (getSpeciesByIdResult == null)
        {
            _logger.LogError("Вид животного с id - {0} не найден", query.SpeciesId);
            return Errors.NotFound($"вид животного с id - {query.SpeciesId}").ToErrorList();
        }

        var breedDtos = getSpeciesByIdResult.Breeds.ToList();
        return breedDtos;
    }
}
