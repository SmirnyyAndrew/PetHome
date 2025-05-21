//using AccountService.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
//using AccountService.IntegrationTests.IntegrationFactories;
//using Microsoft.Extensions.DependencyInjection;
//using PetHome.Core.Application.Interfaces.FeatureManagement;
//using PetHome.SharedKernel.Responses.Dto;
//using Xunit;

//namespace PetHome.Accounts.IntegrationTests.Features.Write.UpdateAccessTokenUsingRefreshToken;
//public class UpdateAccessTokenUsingRefreshTokenUseCaseTest : AccountFactory
//{

//    private readonly ICommandHandler<TokenResponse, UpdateAccessTokenUsingRefreshTokenCommand> _sut;

//    public UpdateAccessTokenUsingRefreshTokenUseCaseTest(IntegrationTestFactory factory) : base(factory)
//    {
//        _sut = _scope.ServiceProvider.GetRequiredService
//            <ICommandHandler<TokenResponse, UpdateAccessTokenUsingRefreshTokenCommand>>();
//    }


//    [Fact]
//    public async void Update_access_token_using_refresh_token()
//    {
//        //array  
//        await SeedRoles();
//        //RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
//        ////var getUserIdResult = await _createUserContract.Execute(roleId, CancellationToken.None); 
//        //var accessTokenResult = await _generateAccessTokenContract.Execute(getUserIdResult.Value, CancellationToken.None);
//        //var refreshSessionResult = await _generateRefreshTokenContract.Execute(getUserIdResult.Value, accessTokenResult.Value, CancellationToken.None);

//        UpdateAccessTokenUsingRefreshTokenCommand command =
//            new UpdateAccessTokenUsingRefreshTokenCommand(refreshSessionResult.Value.RefreshToken);

//        //act
//        var result = await _sut.Execute(command, CancellationToken.None);

//        //assert
//        Assert.True(result.IsSuccess);
//    }
//}
