using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;
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

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))//|| (!Regex.IsMatch(value, PhoneNumberRegex)))
            return Errors.Validation("Номер телефона");

        return new PhoneNumber(value);
    }
}
