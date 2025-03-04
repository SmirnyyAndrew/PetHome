using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using PetHome.Species.Contracts.Messaging;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Application.Features.Write.CreateSpecies;
public class CreateSpeciesUseCase
    : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public CreateSpeciesUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesUseCase> logger, 
        IPublishEndpoint publisher,
       [FromKeyedServices(Constants.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IValidator<CreateSpeciesCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        CreateSpeciesCommand createSpeciesCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(createSpeciesCommand, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        var getByNameResult = await _speciesRepository.GetByName(createSpeciesCommand.SpeciesName, ct);
        if (getByNameResult.IsSuccess)
            return Errors.Conflict(createSpeciesCommand.SpeciesName).ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);

        var speciesResult = _Species.Create(createSpeciesCommand.SpeciesName);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
       
        _Species species = speciesResult.Value;
        var addResult = await _speciesRepository.Add(species, ct);
        if (addResult.IsFailure)
            return addResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(ct);

        CreatedSpeciesEvent createdSpeciesEvent = new CreatedSpeciesEvent(
            species.Id,
            species.Name);
        await _publisher.Publish(createdSpeciesEvent, ct);

        transaction.Commit();

        _logger.LogInformation("Вид животного с именем {0} добавлен", createSpeciesCommand.SpeciesName);
        return addResult.Value;
    }
}
