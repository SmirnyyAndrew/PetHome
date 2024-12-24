using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Extentions;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCase
{
    private readonly IReadDBContext _readDBContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesByIdUseCase> _logger;

    public DeleteSpeciesByIdUseCase(
        IReadDBContext readDBContext,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork,
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
        var isBreedInUse = await _readDBContext.Pets
            .Select(b => b.SpeciesId == command.SpeciesId)
            .FirstOrDefaultAsync();
        if (isBreedInUse)
        {
            string message = $"Не удалось удалить вид - {command.SpeciesId}, так как питомец(-цы) с данным видом уже существует";
            _logger.LogError(message);
            return Errors.Conflict(message).ToErrorList();
        }

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _speciesRepository.RemoveById(command.SpeciesId, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Вид питомца с id - {command.SpeciesId} и его породы удалены";
            _logger.LogInformation(message);
            return message;

        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить вид питомца");
            return Errors.Failure("Database.is.failed").ToErrorList();
        }
    }
}
