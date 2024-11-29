using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class SpeciesName : ValueObject
{
    public string Value { get; }

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

    protected override IEnumerable<object> GetEqualityComponents() 
    {
        yield return Value;
    }
}
