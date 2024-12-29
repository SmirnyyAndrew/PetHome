using CSharpFunctionalExtensions;
using PetHome.Core.Response.Error;
using System.Text.RegularExpressions;

namespace PetHome.Core.ValueObjects;

public record Email
{
    private const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public string Value { get; }

    private Email() { }
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string value)
    {
        string email = value.Trim();

        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, EmailRegex))
            return Errors.Validation("Email");

        return new Email(email);
    }
}