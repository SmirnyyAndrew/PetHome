using AccountService.API.gRPC;
using AccountService.Application.Features.Write.Registration.RegisterUser;
using AccountService.Domain.Aggregates;
using AccountService.IntegrationTests.IntegrationFactories;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Contracts;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using Xunit;

namespace AccountService.IntegrationTests.Features.gRPC;
public class GetUserEmailByUserIdTest : AccountFactory
{ 
    private readonly AccountGRPCService _accountGRPCService;

    public GetUserEmailByUserIdTest(IntegrationTestFactory factory) : base(factory)
    { 
        _accountGRPCService = _scope.ServiceProvider.GetRequiredService<AccountGRPCService>();
    }

    [Fact]
    public async void Get_user_email_by_user_id()
    {
        //array
        await SeedRoles();
        await SeedUsers();
        var user = _dbContext.Users.First();

        GetUserEmailByIdRequest request = new()
        {
            Id = user.Id.ToString(),
        };

        //act
        var result = await _accountGRPCService.GetUserEmailById(request, default);
        string email = result.Email;

        //assert
        Assert.True(email == user.Email);
    }
}

