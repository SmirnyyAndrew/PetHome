using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreateSpeciesVolunteer;
public class CreateSpeciesUseCase
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesUseCase> _logger;

    public CreateSpeciesUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesUseCase> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Execute(
        string speciesName,
        CancellationToken ct)
    {
        var speciesResult = Species.Create(speciesName);
        if (speciesResult.IsFailure)
            return speciesResult.Error;

        var getByNameResult = await _speciesRepository.GetByName(speciesName, ct);
        if (getByNameResult.IsSuccess)
            return Errors.Conflict(speciesName);

        var addResult = await _speciesRepository.Add(speciesResult.Value, ct);
        if (addResult.IsFailure)
            return addResult.Error;

        _logger.LogInformation($"Вид животного с именем {speciesName} добавлен");
        return addResult.Value;
    }
}
