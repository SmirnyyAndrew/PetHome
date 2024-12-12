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
        CreateBreedRequst createBreedRequst,
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

            List<Breed> breeds = new List<Breed>();
            foreach (var breed in createBreedRequst.Breeds)
            {
                var createBreedResult = Breed.Create(breed, createBreedRequst.SpeciesId);
                bool isNotUniqueBreed = species.Breeds.Select(x => x.Name.Value.ToLower() == breed.ToLower()).Any();
                if (isNotUniqueBreed)
                    return Errors.Conflict($"Порода с именем {breed} уже существует");

                if (createBreedResult.IsFailure)
                    return createBreedResult.Error;

                breeds.Add(createBreedResult.Value);
            }

            species.UpdateBreeds(breeds);

            var updateBreedResult = _speciesRepository.Update(species, ct).Result;

            string breedsInLine = string.Join(", ", breeds.Select(x => x.Name).ToList());

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation($"Породы {breedsInLine} добавлена(-ы)");
            return updateBreedResult;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation($"Не удалось создать породу животного");
            return Errors.Failure("Database.is.failed");
        }
    }
}
