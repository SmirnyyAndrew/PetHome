using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Species;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;
public class CreateBreedUseCase
    : ICommandHandler<Guid, CreateBreedCommand>
{
    private readonly ISpeciesRepository SpeciesRepository;
    private readonly ILogger<CreateBreedUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public CreateBreedUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateBreedUseCase> logger,
        IPublishEndpoint publisher,
         IUnitOfWork unitOfWork,
        IValidator<CreateBreedCommand> validator)
    {
        SpeciesRepository = speciesRepository;
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

        var getSpeciesByIdResult = await SpeciesRepository.GetById(createBreedCommand.SpeciesId, ct);
        if (getSpeciesByIdResult.IsFailure)
            return Errors.NotFound($"Вид животного с id {createBreedCommand.SpeciesId} не найден").ToErrorList();

        Species species = getSpeciesByIdResult.Value;
        var updateBreedResult = species.UpdateBreeds(createBreedCommand.Breeds);
        if (updateBreedResult.IsFailure)
            return updateBreedResult.Error.ToErrorList();

        var updateRepositoryResult = await SpeciesRepository.Update(species, ct);

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
