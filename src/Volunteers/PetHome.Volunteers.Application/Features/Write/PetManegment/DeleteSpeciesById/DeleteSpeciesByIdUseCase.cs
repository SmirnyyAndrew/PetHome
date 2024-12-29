using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using PetHome.Volunteers.Application.Database;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCase
    : ICommandHandler<string, DeleteSpeciesByIdCommand>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesByIdUseCase> _logger;

    public DeleteSpeciesByIdUseCase(
        IVolunteerReadDbContext readDBContext,
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
