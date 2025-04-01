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
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Species;
using Species = PetManagementService.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateSpecies;
public class CreateSpeciesUseCase
    : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ISpeciesRepository SpeciesRepository;
    private readonly ILogger<CreateSpeciesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public CreateSpeciesUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesUseCase> logger, 
        IPublishEndpoint publisher,
        IUnitOfWork unitOfWork,
        IValidator<CreateSpeciesCommand> validator)
    {
        SpeciesRepository = speciesRepository;
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

        var getByNameResult = await SpeciesRepository.GetByName(createSpeciesCommand.SpeciesName, ct);
        if (getByNameResult.IsSuccess)
            return Errors.Conflict(createSpeciesCommand.SpeciesName).ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);

        var speciesResult = Species.Create(createSpeciesCommand.SpeciesName);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        Species species = speciesResult.Value;
        var addResult = await SpeciesRepository.Add(species, ct);
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
