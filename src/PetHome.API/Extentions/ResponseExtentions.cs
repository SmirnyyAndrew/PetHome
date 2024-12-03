using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetHome.API.Response;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Extentions;
// Позволяет автоматически опредлетить status code ошибки в контроллерах
public static class ResponseExtentions
{
    public static ActionResult GetSatusCode(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        }; 

        var envelope = ResponseEnvelope.Error(new List<Error> { error });

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
}
