using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class VO_Date : ValueObject
{
    public DateOnly Value { get; }

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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
