namespace PetHome.SharedKernel.Responses.ErrorManagement;
public static class Errors
{
    public static Error Validation(string value)
    {
        string checkedValue = string.IsNullOrWhiteSpace(value) ? "EmptyValue" : value;
        return Error.Validation("Value.is.invalid", checkedValue, $"\"{checkedValue}\" - входные данные некорректны.");
    }

    public static Error NotFound(string value)
    {
        string checkedValue = string.IsNullOrEmpty(value) ? "EmptyValue" : value;
        return Error.NotFound("Value.is.notfound", checkedValue, $"Значение \"{checkedValue}\" не найдено. Проверьте входные данные.");
    }

    public static Error Failure(string message)
    {
        string checkedMessage = string.IsNullOrEmpty(message) ? "EmptyValue" : message;
        return Error.Failure("Value.is.failure", checkedMessage, "\"Проверьте входные данные.");
    }

    public static Error Conflict(string value)
    {
        string checkedValue = string.IsNullOrEmpty(value) ? "EmptyValue" : value;
        return Error.Conflict("Value.is.conflict", checkedValue, $"Значение \"{checkedValue}\" вызвало конфликт. Проверьте входные данные.");
    }
}
