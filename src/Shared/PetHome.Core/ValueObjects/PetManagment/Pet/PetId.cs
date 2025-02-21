using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.PetManagment.Pet;
public record PetId : IComparable<PetId>
{
    public Guid Value { get; }

    private PetId() { }
    private PetId(Guid value)
    {
        Value = value;
    }

    public static Result<PetId, Error> Create() => new PetId(Guid.NewGuid());

    public static Result<PetId, Error> Create(Guid id) => new PetId(id);

    public static Result<PetId, Error> CreateEmpty() => new PetId(Guid.Empty);

    public int CompareTo(PetId? other) => 0;

    public static implicit operator Guid(PetId petId)
    {
        if (petId == null)
            throw new ArgumentNullException();

        return petId.Value;
    }
}