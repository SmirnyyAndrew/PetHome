using FluentValidation;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenValidator
    : AbstractValidator<UpdateAccessTokenUsingRefreshTokenCommand>
{
    public UpdateAccessTokenUsingRefreshTokenValidator()
    {
        RuleFor(t => t.AccessToken).Must(l => l.Trim().Length > 20).WithError(Errors.Validation("Access token"));
    }
}
