using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient
{
    /// <summary>
    /// Проинициализировать имена
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task<MinioFileName> InitName(string filePath)
    {
        //Расширение файла
        string fileExtension = Path.GetExtension(filePath);
        Guid newFilePath = Guid.NewGuid();
        string fullName = newFilePath + fileExtension;
        MinioFileName minioFileName = MinioFileName.Create(fullName).Value;
        return minioFileName;
    }
}
