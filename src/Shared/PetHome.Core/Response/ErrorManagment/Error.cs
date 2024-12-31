namespace PetHome.Core.Response.ErrorManagment;
public record Error
{
    public string Code { get; }
    public string InvalidField { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, string invalidField, ErrorType type)
    {
        Code = code;
        Message = message;
        InvalidField = invalidField;
        Type = type;
    }

    public static Error Validation(string code, string invalidField, string message) => new Error(code, message, invalidField, ErrorType.Validation);
    public static Error NotFound(string code, string invalidField, string message) => new Error(code, message, invalidField, ErrorType.NotFound);
    public static Error Failure(string code, string invalidField, string message) => new Error(code, message, invalidField, ErrorType.Failure);
    public static Error Conflict(string code, string invalidField, string message) => new Error(code, message, invalidField, ErrorType.Conflict);
    public override string ToString() => $"Code: \"{Code}\"\nMessage: \"{Message}\"\nType: \"{Type.ToString()}\"";
}