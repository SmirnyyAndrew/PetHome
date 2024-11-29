using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetHome.Domain.GeneralValueObjects;

public record Email
{
    private const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public string Value { get; }

    private Email() { }
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>("Введите email");

        if (!Regex.IsMatch(value, EmailRegex))
            return Result.Failure<Email>("Email не соответствует формату");

        return new Email(value);
    }
}