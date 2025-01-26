using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Species.Application.Database;
using PetHome.Species.Contracts;

namespace PetHome.Species.Application.Features.Contracts;
public class GetSpeciesIdUsingContract : IGetSpeciesIdContract
{
    private readonly ISpeciesRepository _repository; 

    public GetSpeciesIdUsingContract(
        ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SpeciesId, Error>> Execute(string name, CancellationToken ct)
    {  
      var result =  await _repository.GetByName(name, ct);
        if (result.IsFailure)
            return result.Error;

        var id = result.Value.Id;
        return id;
    }
}
