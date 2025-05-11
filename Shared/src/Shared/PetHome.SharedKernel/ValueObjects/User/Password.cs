using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.User;
public record Password
{
    public string Value { get; }
    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Password");

        return new Password(value);
    }

    public static implicit operator string(Password password) => password.Value;
}
