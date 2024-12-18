using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.Volunteers.PetManegment.CreateSpecies;
public class CreateSpeciesUseCase
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSpeciesCommand> _validator;

    public CreateSpeciesUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesUseCase> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreateSpeciesCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        CreateSpeciesCommand createSpeciesCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(createSpeciesCommand, ct);
        if (validationResult.IsValid is false)
            return (ErrorList)validationResult.Errors;

        var speciesResult = Species.Create(createSpeciesCommand.SpeciesName);
        if (speciesResult.IsFailure)
            return (ErrorList)speciesResult.Error;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getByNameResult = await _speciesRepository.GetByName(createSpeciesCommand.SpeciesName, ct);
            if (getByNameResult.IsSuccess)
                return (ErrorList)Errors.Conflict(createSpeciesCommand.SpeciesName);

            var addResult = await _speciesRepository.Add(speciesResult.Value, ct);
            if (addResult.IsFailure)
                return (ErrorList)addResult.Error;

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Вид животного с именем {0} добавлен", createSpeciesCommand.SpeciesName);
            return addResult.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось создать вид питомца");
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
