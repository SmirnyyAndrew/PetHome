using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.PetManagment.Species;

public record SpeciesId
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

    public static implicit operator Guid(SpeciesId speciesId)
    {
        if (speciesId == null)
            throw new ArgumentNullException();

        return speciesId.Value;
    }
}