using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class SpeciesName : ValueObject
{
    private string Name { get; }

    private SpeciesName(string name)
    {
        Name = name;
    }

    public static Result<SpeciesName> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Failure<SpeciesName>("Строка не должна быть пустой");

        return new SpeciesName(name);
    }

    protected override IEnumerable<object> GetEqualityComponents() 
    {
        yield return Name;
    }
}
