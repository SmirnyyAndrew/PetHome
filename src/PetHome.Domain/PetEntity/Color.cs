using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class Color : ValueObject
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Color>("Введите цвет");

        return new Color(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
