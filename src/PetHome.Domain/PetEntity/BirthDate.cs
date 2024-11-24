using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class BirthDate : ValueObject
{
    private DateOnly Date { get; }

    private BirthDate(DateOnly date)
    {
        Date = date;
    }

    public Result<BirthDate> Create(DateOnly date)
    {
        if (date == null || date.Year > 100)
        {
            return Result.Failure<BirthDate>("Введите корректную дату");
        }

        return new BirthDate(Date);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date;
    }
}
