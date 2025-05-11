using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.User;
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
