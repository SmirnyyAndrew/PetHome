
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetHome.Application.Interfaces;
using Xunit;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class FileProviderFactory : BaseFactory, IClassFixture<IntegrationTestFactory>
{
    protected readonly IFilesProvider _fileServiceMock = Substitute.For<IFilesProvider>();
    private static IntegrationTestFactory _factory;

    public FileProviderFactory(IntegrationTestFactory factory) : base(factory)
    {
        _factory = factory;
        _fileServiceMock = factory.Services.GetRequiredService<IFilesProvider>();
        _factory.SetupSuccessFileServiceMock();
    }

    public static void SetFileServiceSuccess() => _factory.SetupSuccessFileServiceMock();
    public static void SetFileServiceFailed() => _factory.SetupFailedFileServiceMock();
}
