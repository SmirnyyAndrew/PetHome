using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.User;
public record Login
{
    private string Value { get; }
    public Login(string value)
    {
        Value = value;
    }

    public static Result<Login, Error> Create(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Login");

        return new Login(value);
    }
}
