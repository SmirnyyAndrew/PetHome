using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Accounts.Domain.Tokens.RefreshToken;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using System.Security.Claims;

namespace PetHome.Accounts.Application;
public interface ITokenProvider
{
    public Result<RefreshSession, ErrorList> GenerateRefreshToken(User user, string accessToken);
    public string GenerateAccessToken(User user); 
    public IReadOnlyList<Claim> GetClaims(string accessToken);
    public Result<Guid, Error> GetUserId(string accessToken);
    public Result<Guid, Error> GetJti(string accessToken);
}
