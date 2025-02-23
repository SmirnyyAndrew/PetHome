using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion.Message;

namespace PetHome.Core.ValueObjects.PetManagment.Breed;
public record BreedName
{
    public string Value { get; }

    private BreedName() { }
    private BreedName(string value)
    {
        Value = value;
    }

    public static Result<BreedName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Порода");

        return new BreedName(value);
    }

    public static implicit operator string(BreedName name) => name.Value;
}
