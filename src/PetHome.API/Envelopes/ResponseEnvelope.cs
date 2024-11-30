using PetHome.Domain.Shared.Error;

namespace PetHome.API.Response;
public class ResponseEnvelope
{
    public object? Result {  get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public DateTime TimeGenerated { get; }

    private ResponseEnvelope(object? result, Error error)
    {
        Result = result;
        ErrorCode = error.Code;
        ErrorMessage = error.Message;
        TimeGenerated = DateTime.Now;
    }

    public static ResponseEnvelope Ok(object? result) => new ResponseEnvelope(result, null);
    public static ResponseEnvelope Error(Error error) => new ResponseEnvelope(null, error);
}
