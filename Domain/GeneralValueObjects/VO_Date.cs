using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.GeneralValueObjects;
public record VO_Date
{
    public DateOnly Value { get; }

    private VO_Date() { }
    private VO_Date(DateOnly value)
    {
        Value = value;
    }

    public static Result<VO_Date,Error> Create(DateOnly value)
    {
        if (value == null || value.Year > 100)
            return Errors.Validation("Дата");

        return new VO_Date(value);
    }
}
