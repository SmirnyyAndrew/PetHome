using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public record SpeciesName 
{ 
    public string Value { get; }

    private SpeciesName() { }
    private SpeciesName(string value)
    {
        Value = value;
    }

    public static Result<SpeciesName> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<SpeciesName>("Строка не должна быть пустой");

        return new SpeciesName(value);
    } 
}
