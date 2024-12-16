using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;
public class CreateBreedUseCase
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateBreedUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBreedUseCase(
        ISpeciesRepository speciesRepository,
        ILogger<CreateBreedUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Execute(
        CreateBreedCommand createBreedRequst,
        CancellationToken ct)
    {
        //Использование транзакции через UnitOfWork
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getSpeciesByIdResult = await _speciesRepository.GetById(createBreedRequst.SpeciesId, ct);
            if (getSpeciesByIdResult.IsFailure)
                return Errors.NotFound($"Вид животного с id {createBreedRequst.SpeciesId} не найден");

            Species species = getSpeciesByIdResult.Value;
            var updateBreedResult = species.UpdateBreeds(createBreedRequst.Breeds);
            if (updateBreedResult.IsFailure)
                return updateBreedResult.Error;

            var updateRepositoryResult = await _speciesRepository.Update(species, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string breedsInLine = string.Join(", ", createBreedRequst.Breeds);
            _logger.LogInformation("Породы {0} добавлена(-ы)", breedsInLine);
            return updateRepositoryResult;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось создать породу животного\n\r{0}", ex);
            return Errors.Failure("Database.is.failed");
        }
    }
}
