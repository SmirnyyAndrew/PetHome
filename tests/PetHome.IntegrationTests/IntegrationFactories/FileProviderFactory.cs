
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetHome.Core.Interfaces;
using Xunit;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class FileProviderFactory : VolunteerFactory
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
