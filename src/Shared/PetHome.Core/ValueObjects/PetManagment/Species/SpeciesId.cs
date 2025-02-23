using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.PetManagment.Species;

public class SpeciesId : ComparableValueObject
{
    public Guid Value { get; }

    private SpeciesId() { }
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static Result<SpeciesId, Error> Create() => new SpeciesId(Guid.NewGuid());

    public static Result<SpeciesId, Error> Create(Guid id) => new SpeciesId(id);

    public static Result<SpeciesId, Error> CreateEmpty() => new SpeciesId(Guid.Empty);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(SpeciesId speciesId) => speciesId.Value;
}