using PetHome.Accounts.Contracts.User;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Contracts;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Contracts.CreateVolunteerRequest;
public class CreateVolunteerRequestUsingContract : ICreateVolunteerRequestContract
{
    private readonly ICreateUserContract _createUserContract;
    private readonly IGetRoleContract _getRoleContract;

    public CreateVolunteerRequestUsingContract(
        ICreateUserContract createUserContract,
        IGetRoleContract getRoleContract)
    {
        _createUserContract = createUserContract;
        _getRoleContract = getRoleContract;
    }
      
    public async Task<VolunteerRequestId> Execute(CancellationToken ct)
    {
        RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
        UserId userId = await _createUserContract.Execute(roleId, CancellationToken.None);
        VolunteerInfo volunteerInfo = VolunteerInfo.Create("info").Value;

        VolunteerRequest request = VolunteerRequest.Create(userId, volunteerInfo);
        VolunteerRequestId volunteerRequestId = VolunteerRequestId.Create(request.Id).Value;
        return volunteerRequestId;
    }
}
