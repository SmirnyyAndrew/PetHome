using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.SpeciesManagment.BreedEntity;
public class BreedId : ComparableValueObject
{
    public Guid Value { get; }

    private BreedId() { }
    private BreedId(Guid value)
    {
        Value = value;
    }

    public static Result<BreedId, Error> Create() => new BreedId(Guid.NewGuid());

    public static Result<BreedId, Error> Create(Guid id) => new BreedId(id);

    public static Result<BreedId, Error> CreateEmpty() => new BreedId(Guid.Empty);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(BreedId breedId) => breedId.Value;
}