using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreateSpeciesVolunteer;
public class CreateSpeciesUseCase
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Execute(
        string speciesName,
        CancellationToken ct)
    {
        var speciesResult = Species.Create(speciesName);
        if (speciesResult.IsFailure)
            return speciesResult.Error;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getByNameResult = await _speciesRepository.GetByName(speciesName, ct);
            if (getByNameResult.IsSuccess)
                return Errors.Conflict(speciesName);

            var addResult = await _speciesRepository.Add(speciesResult.Value, ct);
            if (addResult.IsFailure)
                return addResult.Error;

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation($"Вид животного с именем {speciesName} добавлен");
            return addResult.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation($"Не удалось создать вид питомца");
            return Errors.Failure("Database.is.failed");
        }
    }
}
