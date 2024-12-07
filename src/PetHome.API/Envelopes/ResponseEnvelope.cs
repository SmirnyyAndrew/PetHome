using FluentValidation.Results;
using Minio.DataModel;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Response;
public class ResponseEnvelope
{
    public IReadOnlyList<Error> Errors { get; }
    public object? Result { get; }
    public DateTime TimeGenerated { get; }

    private ResponseEnvelope(object? result, IEnumerable<Error> errors)
    {
        Errors = errors?.ToList();
        Result = result;
        TimeGenerated = DateTime.Now;
    }

    public static ResponseEnvelope Ok(object? result) => new ResponseEnvelope(result, null);
    public static ResponseEnvelope Error(Error error) => new ResponseEnvelope(null, new List<Error>() { error });
    public static ResponseEnvelope Error(IEnumerable<Error> errors) => new ResponseEnvelope(null, errors);
    public static ResponseEnvelope Error(IEnumerable<ValidationFailure> validationFailures)
    {
        List<Error> errors = new();
        foreach (var failure in validationFailures)
        {
            Error error = Domain.Shared.Error.Errors.Validation(failure.ErrorMessage);
            errors.Add(error);
        }

        return Error(errors);
    }

    public static ResponseEnvelope ConvertObjectStat(ObjectStat objectStat)
    {
        var result = new
        {
            objectStat.ObjectName,
            objectStat.Size,
            objectStat.LastModified
        };

        return new ResponseEnvelope(result, null);
    }
}
