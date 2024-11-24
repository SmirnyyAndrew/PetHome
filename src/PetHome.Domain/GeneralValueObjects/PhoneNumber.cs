using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetHome.Domain.GeneralValueObjects;
public class PhoneNumber : ValueObject
{
    private const string PhoneNumberRegex = @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$";
    private string Number { get; }

    private PhoneNumber(string number)
    {
        Number = number;
    }

    public Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Result.Failure<PhoneNumber>("Введите номер телефона");

        if (!Regex.IsMatch(number, PhoneNumberRegex))
            return Result.Failure<PhoneNumber>("Номер телефона не соответствует формату");

        return new PhoneNumber(number);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
