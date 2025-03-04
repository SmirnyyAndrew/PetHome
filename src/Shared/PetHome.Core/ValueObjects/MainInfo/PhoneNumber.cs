using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion.Message;
using System.Text.RegularExpressions;

namespace PetHome.Core.ValueObjects.MainInfo;
public record PhoneNumber
{
    private const string PhoneNumberRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
    public string Value { get; }

    private PhoneNumber() { }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        string number = value.Trim();

        if (string.IsNullOrWhiteSpace(number) || !Regex.IsMatch(number, PhoneNumberRegex))
            return Errors.Validation("Номер телефона");

        return new PhoneNumber(number);
    }

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}
