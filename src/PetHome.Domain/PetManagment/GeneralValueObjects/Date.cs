using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.PetManagment.GeneralValueObjects;
public record Date
{
    private const int MAX_YEAR = 3000;
    private const int MIN_YEAR = 1900;

    public DateTime Value { get; }

    private Date() { }
    private Date(DateTime value)
    {
        Value = value;
    }

    public static Result<Date, Error> Create(DateTime value)
    {
        if (value == null || value.Year > MAX_YEAR || value.Year < MIN_YEAR)
            return Errors.Validation("Дата");

        return new Date(value);
    }
}
