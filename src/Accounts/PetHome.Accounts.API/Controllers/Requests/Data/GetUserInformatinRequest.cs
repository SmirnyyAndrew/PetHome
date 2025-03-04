using PetHome.Accounts.Application.Features.Read.GetUserInformation;

namespace PetHome.Accounts.API.Controllers.Requests.Data;
public record GetUserInformationRequest(Guid UserId)
{
    public static implicit operator GetUserInformationQuery(GetUserInformationRequest request)
        => new(request.UserId);
}
