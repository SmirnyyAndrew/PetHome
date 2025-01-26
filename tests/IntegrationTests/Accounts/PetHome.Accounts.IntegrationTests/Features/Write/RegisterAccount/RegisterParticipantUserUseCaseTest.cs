using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Features.Write.RegisterAccount;
using PetHome.Accounts.IntegrationTests.IntegrationFactories;
using PetHome.Core.Interfaces.FeatureManagment;
using Xunit;

namespace PetHome.Accounts.IntegrationTests.Features.Write.RegisterAccount;
public class RegisterParticipantUserUseCaseTest : AccountFactory
{
    private readonly ICommandHandler<RegisterParticipantUserCommand> _sut;

    public RegisterParticipantUserUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<RegisterParticipantUserCommand>>();
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

        RegisterParticipantUserCommand command = new RegisterParticipantUserCommand(
            email, name, password);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
