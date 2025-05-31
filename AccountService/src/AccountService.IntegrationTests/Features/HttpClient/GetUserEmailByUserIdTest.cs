using AccountService.Application.Features.Write.Registration.RegisterUser;
using AccountService.Domain.Aggregates;
using AccountService.IntegrationTests.IntegrationFactories;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using Xunit;

namespace AccountService.IntegrationTests.Features.HttpClient;
public class GetUserEmailByUserIdTest : AccountFactory
{
    private readonly ICommandHandler<User, RegisterUserCommand> _sut;

    public GetUserEmailByUserIdTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<User, RegisterUserCommand>>();

    }

    [Fact]
    public async void Get_user_email_by_user_id()
    {
        //array


        //act

        //assert
    }
}

