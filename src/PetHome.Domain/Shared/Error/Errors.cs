namespace PetHome.Domain.Shared.Error;
public static class Errors
{
    public static Error Validation(string value)
    {
        string checkedValue = string.IsNullOrWhiteSpace(value) ? "Value" : value;
        return Error.Validation("Value.is.invalid", checkedValue, $"\"{checkedValue}\" - проверьте входные данные.");
    }

    public static Error NotFound(string value)
    {
        string checkedValue = string.IsNullOrEmpty(value) ? "М" : value;
        return Error.NotFound("Value.is.notfound", checkedValue, $"Значение \"{checkedValue}\" не найдено. Проверьте входные данные.");
    }

    public static Error Failure(string value)
    {
        string checkedValue = string.IsNullOrEmpty(value) ? "Value" : value;
        return Error.Failure("Value.is.failure", checkedValue, "\"Ошибка. Проверьте входные данные.");
    }

    public static Error Conflict(string value)
    {
        string checkedValue = string.IsNullOrEmpty(value) ? "Value" : value;
        return Error.Conflict("Value.is.conflict", checkedValue, $"Значение \"{checkedValue}\" вызвало конфликт. Проверьте входные данные.");
    }
}
