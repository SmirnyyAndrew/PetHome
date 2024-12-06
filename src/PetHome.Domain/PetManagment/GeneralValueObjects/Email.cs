using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;
using System.Text.RegularExpressions;

namespace PetHome.Domain.PetManagment.GeneralValueObjects;

public record Email
{
    private const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public string Value { get; }

    private Email() { }
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email,Error> Create(string value)
    {
        string email = value.Trim();

        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, EmailRegex))
            return Errors.Validation("Email");

        return new Email(email);
    }
}