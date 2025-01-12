using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.PetManagment.Pet;
public record PetShelterId
{
    public Guid Value { get; }

    private PetShelterId() { }
    private PetShelterId(Guid value)
    {
        Value = value;
    }

    public static Result<PetShelterId, Error> Create() => new PetShelterId(Guid.NewGuid());

    public static Result<PetShelterId, Error> Create(Guid id) => new PetShelterId(id);

    public static Result<PetShelterId, Error> CreateEmpty() => new PetShelterId(Guid.Empty);

    public static implicit operator Guid(PetShelterId petShelterId)
    {
        if (petShelterId == null)
            throw new ArgumentNullException();

        return petShelterId.Value;
    }
}