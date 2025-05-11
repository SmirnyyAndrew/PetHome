using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.PetManagment.PetEntity;
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
