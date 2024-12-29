using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public record SpeciesName
{
    public string Value { get; }

    private SpeciesName() { }
    private SpeciesName(string value)
    {
        Value = value;
    }

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Errors.Validation("Вид питомца");

        return new SpeciesName(value);
    }
}
