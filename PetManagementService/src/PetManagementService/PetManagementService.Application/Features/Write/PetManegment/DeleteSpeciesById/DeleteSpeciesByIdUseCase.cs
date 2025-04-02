using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;

namespace PetManagementService.Application.Features.Write.PetManegment.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCase
    : ICommandHandler<string, DeleteSpeciesByIdCommand>
{
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ISpeciesRepository SpeciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesByIdUseCase> _logger;

    public DeleteSpeciesByIdUseCase(
        IPetManagementReadDbContext readDBContext,
        ISpeciesRepository speciesRepository,
         IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesByIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        SpeciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Execute(
        DeleteSpeciesByIdCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);

        await SpeciesRepository.RemoveById(command.SpeciesId, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();
         
        string loggMessage = $"Вид питомца с id - {command.SpeciesId} и его породы удалены";
        _logger.LogInformation(loggMessage);
        return loggMessage;
    }
}
