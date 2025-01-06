using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.User;
public record Password
{
    public string Value { get; }
    public Password(string value)
    {
        Value = value;
    }

    public static Result<Password, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Password");

        return new Password(value);
    }
}
