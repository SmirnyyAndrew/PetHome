using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Framework.Database;
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Species;

namespace PetManagementService.Application.Features.Consumers.CreateSpecies;
public class CreateSpeciesConsumer : IConsumer<CreatedSpeciesEvent>
{
    private readonly ISpeciesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesConsumer(
        ISpeciesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CreatedSpeciesEvent> context)
    {
        var command = context.Message;

        Domain.SpeciesManagment.SpeciesEntity.Species species = 
            Domain.SpeciesManagment.SpeciesEntity.Species.Create(command.Name).Value;

        var transaction = await _unitOfWork.BeginTransaction();
        await _repository.Add(species, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
