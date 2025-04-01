using AccountService.Application.Database.Repositories;
using Grpc.Core;
using PetHome.Accounts.Contracts;

namespace AccountService.API.gRPC;
public class AccountGRPCService(IAuthenticationRepository repository)
    : AccountGRPC.AccountGRPCBase
{
    public override async Task<GetUserEmailByIdResponse> GetUserEmailById(
        GetUserEmailByIdRequest request, ServerCallContext context)
    {
        var parseResult = Guid.TryParse(request.Id, out Guid userId);
        if (parseResult is false)
            return new() { Email = string.Empty };
        
        var getUserResult = await repository.GetUserById(userId, CancellationToken.None);
        if(getUserResult.IsFailure)
            return new() { Email = string.Empty };

        GetUserEmailByIdResponse response = new()
        {
            Email = getUserResult.Value.Email
        };
        return response;
    }
}
