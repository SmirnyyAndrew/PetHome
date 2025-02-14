using CSharpFunctionalExtensions;
namespace FilesService.Core.Validation.Validator;

public record ErrorList(List<ErrorManagment.Error> Errors)
{

    public static implicit operator ErrorList(List<ErrorManagment.Error> errors) => new ErrorList(errors);

    public static implicit operator ErrorList(ErrorManagment.Error error)
    {
        var errorListed = new List<ErrorManagment.Error>() { error };
        return new ErrorList(errorListed);
    }

    public static implicit operator ErrorList(List<FluentValidation.Results.ValidationFailure> validationErrors)
    {
        List<ErrorManagment.Error> errors = validationErrors.Select(v =>
                 ErrorManagment.Error.Validation(v.ErrorCode, v.PropertyName, v.ErrorMessage))
                .ToList();
        return new ErrorList(errors);
    }
}