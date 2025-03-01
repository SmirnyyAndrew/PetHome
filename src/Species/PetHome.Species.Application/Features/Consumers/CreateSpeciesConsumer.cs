using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using PetHome.Species.Contracts.Messaging;

namespace PetHome.Species.Application.Features.Consumers;
public class CreateSpeciesConsumer : IConsumer<CreatedSpeciesEvent>
{
    private readonly ISpeciesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesConsumer(
        ISpeciesRepository repository,
        [FromKeyedServices(Constants.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CreatedSpeciesEvent> context)
    {
        var command = context.Message;

        Domain.SpeciesManagment.SpeciesEntity.Species species = 
            Domain.SpeciesManagment.SpeciesEntity.Species.Create(command.SpeciesName).Value;

        var transaction = await _unitOfWork.BeginTransaction();
        await _repository.Add(species, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
