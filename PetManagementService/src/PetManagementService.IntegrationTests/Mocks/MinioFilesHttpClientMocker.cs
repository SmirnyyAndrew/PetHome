using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Moq;

namespace PetManagementService.IntegrationTests.Mocks;
public static class MinioFilesHttpClientMocker
{
    public static IMinioFilesHttpClient MockMethods()
    {
        Mock<IMinioFilesHttpClient> minioClientMock = new Mock<IMinioFilesHttpClient>();

        minioClientMock
            .Setup(x => x.UploadFileWithDataChecking(
                It.IsAny<UploadFileRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(UnitResult.Success<string>());

        minioClientMock
            .Setup(x => x.DeleteFile(
                It.IsAny<MinioFilesInfoDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(UnitResult.Success<string>());

        minioClientMock
            .Setup(x => x.DownloadFiles(
                It.IsAny<DownloadFilesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(UnitResult.Success<string>());

        minioClientMock
            .Setup(x => x.GetFilePresignedPath(
                It.IsAny<MinioFilesInfoDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<string> { "https://minio.local/photos/1.png", "https://minio.local/photos/2.png" });

        minioClientMock
            .Setup(x => x.InitName(It.IsAny<string>()))
            .ReturnsAsync(MinioFileName.Create("some-generated-name.png").Value);

        return minioClientMock.Object;
    }
}
