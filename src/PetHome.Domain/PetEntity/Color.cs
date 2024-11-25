using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class Color : ValueObject
{
    private string Name { get; }

    private Color(string name)
    {
        Name = name;
    }

    public Result<Color> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Color>("Введите цвет");

        return new Color(name);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
