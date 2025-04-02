using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.PetManagment.Pet;
public record PetName
{
    public string Value { get; }

    private PetName() { }
    private PetName(string value)
    {
        Value = value;
    }

    public static Result<PetName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Кличка");

        return new PetName(value);
    }

    public static implicit operator string(PetName name) => name.Value;
}