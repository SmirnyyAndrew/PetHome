using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Infrastructure.Database;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity; 

namespace PetManagementService.Application.Features.Contracts.SpeciesEntity;
public class CreateSpeciesUseCase
{
    private readonly ISpeciesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesUseCase(
        ISpeciesRepository repository,
        [FromKeyedServices(Constants.Database.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SpeciesId, Error>> Execute(string name, CancellationToken ct)
    {
        Species species = Species.Create(name).Value;

        var transaction = await _unitOfWork.BeginTransaction();
        await _repository.Add(species, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return species.Id;
    }
}
