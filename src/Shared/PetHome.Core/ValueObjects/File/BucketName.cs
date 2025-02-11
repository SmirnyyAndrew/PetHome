using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.File;
public record BucketName
{
    public string Value { get; }

    private BucketName() { }
    private BucketName(string value)
    {
        Value = value;
    }

    public static Result<BucketName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("BucketName");

        return new BucketName(value);
    }
}
