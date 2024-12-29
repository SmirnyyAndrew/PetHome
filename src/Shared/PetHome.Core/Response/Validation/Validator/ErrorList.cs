namespace PetHome.Core.Response.Validation.Validator;


public record ErrorList(List<Error> Errors)
{

    public static implicit operator ErrorList(List<Error> errors) => new ErrorList(errors);

    public static implicit operator ErrorList(Error error)
    {
        var errorListed = new List<Error>() { error };
        return new ErrorList(errorListed);
    }

    public static implicit operator ErrorList(List<FluentValidation.Results.ValidationFailure> validationErrors)
    {
        List<Error> errors = validationErrors.Select(v =>
                 Error.Validation(v.ErrorCode, v.PropertyName, v.ErrorMessage))
                .ToList();
        return new ErrorList(errors);
    }
}