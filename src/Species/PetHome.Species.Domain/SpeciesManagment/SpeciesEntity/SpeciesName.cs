using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
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
