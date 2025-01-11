using FluentValidation.Results;
using Minio.DataModel;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Core.Response.ErrorManagment.Envelopes;
public class ResponseEnvelope
{
    public ErrorList Errors { get; }
    public object? Result { get; }
    public DateTime TimeGenerated { get; }

    private ResponseEnvelope(object? result, IEnumerable<Error> errors)
    {
        Errors = errors?.ToList();
        Result = result;
        TimeGenerated = DateTime.UtcNow;
    }

    public static ResponseEnvelope Ok(object? result) => new ResponseEnvelope(result, null);
    public static ResponseEnvelope Error(Error error) => new ResponseEnvelope(null, new List<Error>() { error });
    public static ResponseEnvelope Error(IEnumerable<Error> errors) => new ResponseEnvelope(null, errors);
    public static ResponseEnvelope Error(ErrorList errors) => new ResponseEnvelope(null, errors.Errors);

    public static implicit operator ResponseEnvelope(ObjectStat objectStat)
    {
        var result = new
        {
            objectStat.ObjectName,
            objectStat.Size,
            objectStat.LastModified
        };

        return new ResponseEnvelope(result, null);
    }

    public static implicit operator ResponseEnvelope(
        ValidationFailure[] validationFailures)
    {
        List<Error> errors = new();
        foreach (var failure in validationFailures)
        {
            Error error = ErrorManagment.Errors.Validation(failure.ErrorMessage);
            errors.Add(error);
        }

        return Error(errors.ToArray());
    }
}
