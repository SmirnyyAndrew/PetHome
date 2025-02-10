using FilesService.Core.ErrorManagment;
using FilesService.Core.ErrorManagment.Envelopes;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Extentions.ErrorExtentions;
// Позволяет автоматически опредлетить status code ошибки в контроллерах
public static class ResponseExtentions
{
    public static ActionResult GetStatusCode(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var envelope = ResponseEnvelope.Error(error);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
    public static ActionResult GetStatusCode(this ErrorList errorList)
        => GetStatusCode(errorList.Errors.FirstOrDefault());
}
