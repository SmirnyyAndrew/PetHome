using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.GeneralValueObjects;
public record Date
{
    public DateOnly Value { get; }

    private Date() { }
    private Date(DateOnly value)
    {
        Value = value;
    }

    public static Result<Date,Error> Create(DateOnly value)
    {
        if (value == null || value.Year > 100)
            return Errors.Validation("Дата");

        return new Date(value);
    }
}
