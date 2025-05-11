using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.SpeciesManagment.SpeciesEntity;
public record SpeciesName
{
    public string Value { get; }

    private SpeciesName() { }
    private SpeciesName(string value)
    {
        Value = value;
    }
     
    public static implicit operator string(SpeciesName name) => name.Value;

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Errors.Validation("Вид питомца");

        return new SpeciesName(value);
    }
}
