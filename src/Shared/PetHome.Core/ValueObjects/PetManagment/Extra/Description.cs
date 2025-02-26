using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion.Message;

namespace PetHome.Core.ValueObjects.PetManagment.Extra;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Описание");

        return new Description(value);
    }

    public static implicit operator string(Description desc) => desc.Value;
}