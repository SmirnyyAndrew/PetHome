using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCase
    : ICommandHandler<string, DeleteSpeciesByIdCommand>
{
    private readonly ISpeciesReadDbContext _readDBContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesByIdUseCase> _logger;

    public DeleteSpeciesByIdUseCase(
        ISpeciesReadDbContext readDBContext,
        ISpeciesRepository speciesRepository,
        [FromKeyedServices(Constants.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesByIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Execute(
        DeleteSpeciesByIdCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);

        await _speciesRepository.RemoveById(command.SpeciesId, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();
         
        string loggMessage = $"Вид питомца с id - {command.SpeciesId} и его породы удалены";
        _logger.LogInformation(loggMessage);
        return loggMessage;
    }
}
