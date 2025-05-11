using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.Responses.RefreshToken;
using System.Security.Claims;

namespace AccountService.Application;
public interface ITokenProvider
{
    public Result<RefreshSession, ErrorList> GenerateRefreshToken(User user, RefreshSession oldRefreshSession);
    public Result<RefreshSession, ErrorList> GenerateRefreshToken(User user, string accessToken);
    public string GenerateAccessToken(User user);
    public IReadOnlyList<Claim> GetClaims(string accessToken);
    public Result<Guid, Error> GetUserId(string accessToken);
    public Result<Guid, Error> GetJti(string accessToken);
}
