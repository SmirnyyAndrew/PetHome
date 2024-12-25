using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NSubstitute;
using Xunit;

namespace PetHome.IntegrationTests;
public class FileProviderFactory : BaseTest, IClassFixture<IntegrationTestFactory>
{
    private readonly IFileProvider _fileServiceMock = Substitute.For<IFileProvider>();
    private static IntegrationTestFactory _factory;

    public FileProviderFactory(IntegrationTestFactory factory) : base(factory)
    {
        _factory = factory;
        _fileServiceMock = factory.Services.GetRequiredService<IFileProvider>();
        _factory.SetupSuccessFileServiceMock();
    }

    public static void SetFileServiceSuccess() => _factory.SetupSuccessFileServiceMock();
    public static void SetFileServiceFailed() => _factory.SetupFailedFileServiceMock();
}
