using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
using PetHome.Accounts.IntegrationTests.IntegrationFactories;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Dto;
using Xunit;

namespace PetHome.Accounts.IntegrationTests.Features.Write.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenUseCaseTest : AccountFactory
{

    private readonly ICommandHandler<TokenResponse, UpdateAccessTokenUsingRefreshTokenCommand> _sut;

    public UpdateAccessTokenUsingRefreshTokenUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService
            <ICommandHandler<TokenResponse, UpdateAccessTokenUsingRefreshTokenCommand>>();
    }


    [Fact]
    public async void Update_access_token_using_refresh_token()
    {
        //array 
        Guid refreshToken = Guid.NewGuid();
        string accessToken = Guid.NewGuid().ToString(); 
        UpdateAccessTokenUsingRefreshTokenCommand command = new UpdateAccessTokenUsingRefreshTokenCommand(refreshToken, accessToken);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
