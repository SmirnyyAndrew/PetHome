using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Application.Features.Read.Species.GetAllSpecies;
public class GetAllSpeciesUseCase
    : IQueryHandler<IReadOnlyList<SpeciesDto>>
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

    public async Task<IReadOnlyList<SpeciesDto>> Execute(
        CancellationToken ct)
    {
        IReadOnlyList<SpeciesDto> speciesDto =
            await _readDBContext.Species.ToListAsync(ct);
        return speciesDto;
    } 
}
