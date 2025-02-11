using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.File;
public record MediaFile
{
    public Guid Id { get; }
    public FileType Type { get; }
    public string BucketName { get; }
    public string FileName { get; }

    private MediaFile(){}
    private MediaFile(
        FileType type,
        string bucketName,
        string fileName)
    {
        Id = Guid.NewGuid();
        Type = type;
        BucketName = bucketName;
        FileName = fileName;
    }

    public static Result<MediaFile, Error> Create(
        string bucketName,
        string fileName,
        FileType type = FileType.image)
    {
        if (string.IsNullOrWhiteSpace(bucketName)
            || string.IsNullOrWhiteSpace(fileName))
            return Errors.Validation("Название bucket и файла не должны бать пустыми");

        return new MediaFile(type, bucketName, fileName);
    }
}
