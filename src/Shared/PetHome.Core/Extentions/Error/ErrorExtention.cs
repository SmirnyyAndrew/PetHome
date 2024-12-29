using PetHome.Core.Response.Validation.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Core.Extentions.Error;
public static class ErrorExtention
{
    public static ErrorList ToErrorList(this Error error)
    {
        return new ErrorList([error]);
    }

    public static ErrorList ToErrorList(
        this List<FluentValidation.Results.ValidationFailure> validationErrors)
    {
        List<Error> errors = validationErrors.Select(v =>
                 Error.Validation(v.ErrorCode, v.PropertyName, v.ErrorMessage))
                .ToList();
        return new ErrorList(errors);
    }

}
