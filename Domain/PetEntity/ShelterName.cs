using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;

public record ShelterName
{
    public string Value { get; }
     
    private ShelterName() { } 
    private ShelterName(string value)
    {
        Value = value;
    }

    public static Result<ShelterName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<ShelterName>("Строка не должна быть пустой");

        return new ShelterName(value);
    }
}
