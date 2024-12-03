using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetHome.API.Response;
using PetHome.Domain.Shared.Error;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetHome.API.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails == null)
            throw new InvalidOperationException();


        List<Error> errors = new();
        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            foreach (var error in validationErrors)
            {
                errors.Add(Errors.Validation(error));
                //errors.Add(Error.Validation(invalidField, error));
            }  
        }

        ResponseEnvelope envelope = ResponseEnvelope.Error(errors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
