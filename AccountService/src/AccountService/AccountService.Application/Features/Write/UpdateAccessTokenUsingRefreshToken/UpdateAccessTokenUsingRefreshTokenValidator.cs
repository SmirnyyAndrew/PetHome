using FluentValidation;

namespace AccountService.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenValidator
    : AbstractValidator<UpdateAccessTokenUsingRefreshTokenCommand>
{
    public UpdateAccessTokenUsingRefreshTokenValidator()
    {
        //RuleFor(t => t.AccessToken).Must(l => l.Trim().Length > 20).WithError(Errors.Validation("Access token"));
    }
}
