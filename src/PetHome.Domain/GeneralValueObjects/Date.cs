using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.GeneralValueObjects;
public record Date
{
    private const int MAX_YEAR = 3000;
    private const int MIN_YEAR = 1900;

    public DateOnly Value { get; }

    private Date() { }
    private Date(DateOnly value)
    {
        Value = value;
    }


    public static Result<Date, Error> Create(DateOnly value)
    {
        if (value == null || value.Year > MAX_YEAR || value.Year < MIN_YEAR)
            return Errors.Validation("Дата");

        return new Date(value);
    }
}
