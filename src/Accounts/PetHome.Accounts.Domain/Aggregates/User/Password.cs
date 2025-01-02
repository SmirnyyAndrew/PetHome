using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.User;
public class Password
{
    private string Value { get; set; }
    public Password(string value)
    {
        Value = value;
    }

    public static Result<Password, Error> Create(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Password");

        return new Password(value);
    }
}
