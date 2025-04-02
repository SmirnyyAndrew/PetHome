using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.PetManagment.Pet;

public record ShelterName
{
    public string Value { get; }

    private ShelterName() { }
    private ShelterName(string value)
    {
        Value = value;
    }

    public static Result<ShelterName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Приют");

        return new ShelterName(value);
    }

    public static implicit operator string(ShelterName name) => name.Value;
}
