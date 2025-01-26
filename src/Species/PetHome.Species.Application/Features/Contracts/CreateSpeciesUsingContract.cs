using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using PetHome.Species.Contracts;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Application.Features.Contracts;
public class CreateSpeciesUsingContract : ICreateSpeciesContract
{
    private readonly ISpeciesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesUsingContract(
        ISpeciesRepository repository,
        [FromKeyedServices(Constants.SPECIES_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SpeciesId, Error>> Execute(string name, CancellationToken ct)
    {
        _Species species = _Species.Create(name).Value;

        var transaction = await _unitOfWork.BeginTransaction();
        await _repository.Add(species, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return species.Id;
    }
}
