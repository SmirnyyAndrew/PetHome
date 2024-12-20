using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.DeleteBreedById;
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
        Guid speciesId,
        CancellationToken ct)
    {
        var isBreedInUse = await _readDBContext.Pets
            .Select(b => b.SpeciesId == speciesId)
            .FirstOrDefaultAsync();
        if (isBreedInUse)
        {
            string message = $"Не удалось удалить вид - {speciesId}, так как питомец(-цы) с данным видом уже существует";
            _logger.LogError(message);
            return (ErrorList)Errors.Conflict(message);
        }

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _speciesRepository.RemoveById(speciesId, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Вид питомца с id - {speciesId} и его породы удалены";
            _logger.LogInformation(message);
            return message;

        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить вид питомца");
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
