using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Extentions;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.CreateSpecies;
public class CreateSpeciesUseCase
    : ICommandHandler<Guid, CreateSpeciesCommand>
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
            return validationResult.Errors.ToErrorList();

        var speciesResult = Species.Create(createSpeciesCommand.SpeciesName);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getByNameResult = await _speciesRepository.GetByName(createSpeciesCommand.SpeciesName, ct);
            if (getByNameResult.IsSuccess)
                return Errors.Conflict(createSpeciesCommand.SpeciesName).ToErrorList();

            var addResult = await _speciesRepository.Add(speciesResult.Value, ct);
            if (addResult.IsFailure)
                return addResult.Error.ToErrorList();

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Вид животного с именем {0} добавлен", createSpeciesCommand.SpeciesName);
            return addResult.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось создать вид питомца");
            return Errors.Failure("Database.is.failed").ToErrorList();
        }
    }
}
