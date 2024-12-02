using CSharpFunctionalExtensions;
using FluentValidation.Results;
using PetHome.Domain.Shared.Error;
using System.ComponentModel.DataAnnotations;

namespace PetHome.API.Response;
public class ResponseEnvelope
{
    public IEnumerable<Error> Errors { get; }
    public object? Result { get; }
    public DateTime TimeGenerated { get; }

    private ResponseEnvelope(object? result, IEnumerable<Error> errors)
    {
        Errors = errors;
        Result = result;
        TimeGenerated = DateTime.Now;
    }

    public static ResponseEnvelope Ok(object? result) => new ResponseEnvelope(result, null);
    public static ResponseEnvelope Error(IEnumerable<Error> errors) => new ResponseEnvelope(null, errors);

    public static ResponseEnvelope Error(FluentValidation.Results.ValidationResult validatorResult)
    {
        List<Error> errors = new();
        validatorResult.Errors.ForEach(x => errors.Add(Domain.Shared.Error.Error.Validation(x.ErrorCode, x.ErrorMessage)));
        return ResponseEnvelope.Error(errors);
    }
}
