using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion.Message;

namespace PetHome.Core.ValueObjects.PetManagment.Pet;
public class Color : ValueObject
{
    public string Value { get; }

    private Color() { }
    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Цвет");

        return new Color(value);
    }
     
    public static implicit operator string(Color color) => color.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
