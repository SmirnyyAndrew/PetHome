using CSharpFunctionalExtensions;
using PetHome.Core.Response.Error;

namespace PetHome.Domain.PetManagment.GeneralValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description,Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Описание");

        return new Description(value);
    }
}