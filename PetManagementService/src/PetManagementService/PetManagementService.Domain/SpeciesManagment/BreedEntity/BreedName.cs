using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.SpeciesManagment.BreedEntity;
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
