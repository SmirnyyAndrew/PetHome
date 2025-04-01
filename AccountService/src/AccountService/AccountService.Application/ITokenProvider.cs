using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.Response.Validation.Validator;
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
