using AccountService.Application.Features.Write.Registration.RegisterUser;
using AccountService.Domain.Aggregates;
using AccountService.IntegrationTests.IntegrationFactories;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using Xunit;

namespace AccountService.IntegrationTests.Features.Write.RegisterAccount;
public class RegisterParticipantUserUseCaseTest : AccountFactory
{
    private readonly ICommandHandler<User, RegisterUserCommand> _sut;

    public RegisterParticipantUserUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<User, RegisterUserCommand>>();
    }


    [Fact]
    public async void Register_user()
    {
        //array 
        await SeedRoles();
        var list = _dbContext.Roles.ToList();   

        string email = "email211s12@mail.com";
        string name = "Ivan312ov";
        string password = "Iva1243novPassword1123";

        RegisterUserCommand command = new RegisterUserCommand(
            email, name, password);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
