using PetHome.Core.Interfaces;

namespace PetHome.SharedKernel.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    public MinioFileName InitName(string filePath)
    {
        //Расширение файла
        string fileExtension = Path.GetExtension(filePath);
        Guid newFilePath = Guid.NewGuid();
        string fullName = newFilePath + fileExtension;
        return MinioFileName.Create(fullName).Value;
    }
}
