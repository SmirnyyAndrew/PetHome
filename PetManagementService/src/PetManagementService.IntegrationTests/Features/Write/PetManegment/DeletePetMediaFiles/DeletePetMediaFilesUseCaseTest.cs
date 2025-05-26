using FilesService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Tests.IntegrationTests.Mocks.FileManagement;
using PetManagementService.Application.Dto.Pet;
using PetManagementService.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.DeletePetMediaFiles;

[Collection("Pet")]
public class DeletePetMediaFilesUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<string, DeletePetMediaFilesCommand> _sut;

    public DeletePetMediaFilesUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, DeletePetMediaFilesCommand>>();
    }

    [Fact]
    public async void Delete_pet_media_files()
    {
        //array

        IMinioFilesHttpClient _minioClient = MinioFilesHttpClientMocker.MockMethods();

        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers.SelectMany(p => p.Pets).First();

        //IMinioFilesHttpClient minioServiceMock = MinioFilesHttpClientMock.CreateMockMethods().Object;

        string bucketName = "photos";
        List<string> fileNames = new List<string>() { "99482373434.png", "3245345434.png", "845234123434.png", };
        DeletePetMediaFilesDto deletePetMediaFilesDto = new DeletePetMediaFilesDto(
            pet.Id,
            bucketName,
            fileNames);
        DeletePetMediaFilesCommand command = new DeletePetMediaFilesCommand(
            pet.VolunteerId,
            deletePetMediaFilesDto,
            _minioClient);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
