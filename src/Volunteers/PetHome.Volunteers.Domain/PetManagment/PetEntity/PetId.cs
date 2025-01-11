using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public record class PetId
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

    public static implicit operator Guid(PetId petId)
    {
        if (petId == null)
            throw new ArgumentNullException();

        return petId.Value;
    }
}