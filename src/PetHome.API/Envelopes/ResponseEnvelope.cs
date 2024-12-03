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
    public static ResponseEnvelope Error(IEnumerable<Error> errors) => new ResponseEnvelope(null, errors);
}
