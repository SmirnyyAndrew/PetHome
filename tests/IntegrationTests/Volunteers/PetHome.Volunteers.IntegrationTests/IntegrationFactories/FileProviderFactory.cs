using FilesService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace PetHome.Volunteers.IntegrationTests.IntegrationFactories;
public class FileProviderFactory : VolunteerFactory
{
    protected readonly IMinioFilesHttpClient _fileServiceMock = Substitute.For<IMinioFilesHttpClient>();
    private static IntegrationTestFactory _factory;

    public FileProviderFactory(IntegrationTestFactory factory) : base(factory)
    {
        _factory = factory;
        _fileServiceMock = factory.Services.GetRequiredService<IMinioFilesHttpClient>();
        _factory.SetupSuccessFileServiceMock();
    }

    public static void SetFileServiceSuccess() => _factory.SetupSuccessFileServiceMock();
    public static void SetFileServiceFailed() => _factory.SetupFailedFileServiceMock();
}
