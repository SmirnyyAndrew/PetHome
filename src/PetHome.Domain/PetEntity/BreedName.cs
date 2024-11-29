using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class BreedName : ValueObject
{
    public string Value { get; }

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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

} 
