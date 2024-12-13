using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;

public record MinioFileName
{
    public string Value { get; }

    private MinioFileName(string value)
    {
        Value = value;
    }

    public static Result<MinioFileName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Имя файла");

        return new MinioFileName(value);
    }
}