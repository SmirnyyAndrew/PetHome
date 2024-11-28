using CSharpFunctionalExtensions;

namespace PetHome.Domain.GeneralValueObjects;
public record VO_Date
{
    public DateOnly Value { get; }

    private VO_Date() { }
    private VO_Date(DateOnly value)
    {
        Value = value;
    }

    public static Result<VO_Date> Create(DateOnly value)
    {
        if (value == null || value.Year > 100)
        {
            return Result.Failure<VO_Date>("Введите корректную дату");
        }

        return new VO_Date(value);
    }
}
