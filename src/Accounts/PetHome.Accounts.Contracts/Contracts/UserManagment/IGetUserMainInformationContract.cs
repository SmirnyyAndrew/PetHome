using PetHome.Accounts.Contracts.Dto;

namespace PetHome.Accounts.Contracts.Contracts.UserManagment;
public interface IGetUserMainInformationContract
{
    public Task<UserDto?> Execute(Guid userId, CancellationToken ct); 
}
