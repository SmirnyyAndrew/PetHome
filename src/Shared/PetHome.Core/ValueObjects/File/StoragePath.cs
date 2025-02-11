using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.File;
public record StoragePath
{
    public string Value { get; }

    private StoragePath() { }
    private StoragePath(string value)
    {
        Value = value;
    }

    public static Result<StoragePath, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("StoragePath");

        return new StoragePath(value);
    }
}
