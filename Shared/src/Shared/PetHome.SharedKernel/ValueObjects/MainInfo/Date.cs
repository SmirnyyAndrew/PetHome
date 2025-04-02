using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.MainInfo;
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

        return new Date(value.ToUniversalTime());
    }

    public static Result<Date, Error> Create()
    {
        return new Date(DateTime.UtcNow);
    }

    public static implicit operator DateTime(Date date) => date.Value;
}
