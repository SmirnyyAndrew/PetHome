using CSharpFunctionalExtensions;
using PetHome.Core.Response.Error;

namespace PetHome.Domain.PetManagment.GeneralValueObjects;
public record Media
{
    public string BucketName { get; }
    public string FileName { get; }

    private Media(string bucketName, string fileName)
    {
        BucketName = bucketName;
        FileName = fileName;
    }

    public static Result<Media, Error> Create(string bucketName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(bucketName) || string.IsNullOrWhiteSpace(fileName))
            return Errors.Validation("Название bucket и файла не должны бать пустыми");

        return new Media(bucketName, fileName);
    } 
}
