using FilesService.Core.ErrorManagment;
using Microsoft.AspNetCore.Identity;

namespace FilesService.Extentions.ErrorExtentions;
public static class ErrorExtention
{
    public static ErrorList ToErrorList(this Error error)
    {
        return new ErrorList([error]);
    }

    public static ErrorList ToErrorList(
        this IEnumerable<FluentValidation.Results.ValidationFailure> validationErrors)
    {
        List<Error> errors = validationErrors.Select(v =>
                 Error.Validation(v.ErrorCode, v.PropertyName, v.ErrorMessage))
                .ToList();
        return new ErrorList(errors);
    }

    public static ErrorList ToErrorList(
        this IEnumerable<IdentityError> validationErrors)
    {
        List<Error> errors = validationErrors.Select(v =>
                 Error.Validation(v.Code, "validation.error", v.Description))
                .ToList();
        return new ErrorList(errors);
    }
}
