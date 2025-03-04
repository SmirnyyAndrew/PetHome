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

namespace PetHome.Species.Application.Features.Write.CreateBreed;
public class CreateBreedUseCase
    : ICommandHandler<Guid, CreateBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateBreedUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public CreateBreedUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateBreedUseCase> logger,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IValidator<CreateBreedCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        CreateBreedCommand createBreedCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(createBreedCommand, ct);
        if (validationResult.IsValid == false)
            return validationResult.Errors.ToErrorList();

        //Использование транзакции через UnitOfWork
        var transaction = await _unitOfWork.BeginTransaction(ct);

        var getSpeciesByIdResult = await _speciesRepository.GetById(createBreedCommand.SpeciesId, ct);
        if (getSpeciesByIdResult.IsFailure)
            return Errors.NotFound($"Вид животного с id {createBreedCommand.SpeciesId} не найден").ToErrorList();

        _Species species = getSpeciesByIdResult.Value;
        var updateBreedResult = species.UpdateBreeds(createBreedCommand.Breeds);
        if (updateBreedResult.IsFailure)
            return updateBreedResult.Error.ToErrorList();

        var updateRepositoryResult = await _speciesRepository.Update(species, ct);

        await _unitOfWork.SaveChanges(ct);
      
        CreatedBreedEvent createdBreedEvent = new CreatedBreedEvent(
            species.Id,
            species.Name);
        await _publisher.Publish(createdBreedEvent, ct);

        transaction.Commit();

        string breedsInLine = string.Join(", ", createBreedCommand.Breeds);
        _logger.LogInformation("Породы {0} добавлена(-ы)", breedsInLine);
        return updateRepositoryResult;
    }
}
