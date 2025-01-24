using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.User;
public record Login
{
    public string Value { get; }
    public Login(string value)
    {
        Value = value;
    }

    public static Result<Login, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Login");

        return new Login(value);
    }
}
