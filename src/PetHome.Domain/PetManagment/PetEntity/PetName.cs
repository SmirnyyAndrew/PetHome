using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.PetManagment.PetEntity;
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
}