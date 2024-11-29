using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetHome.Domain.GeneralValueObjects;
public record PhoneNumber
{ 
    private const string PhoneNumberRegex = @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$";
    public string Value { get; }

    private PhoneNumber() { }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<PhoneNumber>("Введите номер телефона");

        if (!Regex.IsMatch(value, PhoneNumberRegex))
            return Result.Failure<PhoneNumber>("Номер телефона не соответствует формату");

        return new PhoneNumber(value);
    }
}
