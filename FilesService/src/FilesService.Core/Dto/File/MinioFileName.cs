namespace FilesService.Core.Dto.File;

using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;

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