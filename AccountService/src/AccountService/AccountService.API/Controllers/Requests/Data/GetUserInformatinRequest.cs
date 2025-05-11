using AccountService.Application.Features.Read.GetUserInformation;

namespace AccountService.API.Controllers.Requests.Data;
public record GetUserInformationRequest(Guid UserId)
{
    public static implicit operator GetUserInformationQuery(GetUserInformationRequest request)
        => new(request.UserId);
}
