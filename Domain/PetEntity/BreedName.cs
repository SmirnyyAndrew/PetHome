using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public record BreedName
{ 
    public string Value { get; }

    private BreedName() { }
    private BreedName(string value)
    {
        Value = value;
    }

    public static Result<BreedName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<BreedName>("Строка не должна быть пустой");

        return new BreedName(value);
    }
} 
