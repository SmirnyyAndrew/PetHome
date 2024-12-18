namespace PetHome.Domain.Shared.Error;


public record ErrorList
{
    public IReadOnlyList<Error> Errors { get; private set; } = new List<Error>();

    public static implicit operator ErrorList(List<Error> errors) => new ErrorList(errors);
    public static implicit operator ErrorList(Error error) => new ErrorList(error);
    public static implicit operator ErrorList(List<FluentValidation.Results.ValidationFailure> validationErrors)
    {
        List<Error> errors = validationErrors.Select(v =>
                 Error.Validation(v.ErrorCode, v.PropertyName, v.ErrorMessage))
                .ToList();
        return new ErrorList(errors);
    }
}