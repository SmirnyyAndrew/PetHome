using PetHome.Accounts.Contracts;

namespace NotificationService.Application.gRPC;

public class AccountGRPCService
{
    private readonly AccountGRPC.AccountGRPCClient _accountGRPC;
     
    public AccountGRPCService(AccountGRPC.AccountGRPCClient accountGRPC)
    { 
        _accountGRPC = accountGRPC;
    }

    public async Task<GetUserEmailByIdResponse> GetUserEmailById(GetUserEmailByIdRequest request)
    { 
        GetUserEmailByIdResponse response = await _accountGRPC.GetUserEmailByIdAsync(request);
        return response;
    }
}
