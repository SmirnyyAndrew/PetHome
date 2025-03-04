using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetHome.Core.ValueObjects.User;
public record Login
{
    public string Value { get; }
    private Login(string value)
    {
        Value = value;
    }

    public static Result<Login, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Login");

        return new Login(value);
    }

    public static implicit operator string(Login login) => login.Value;
}
