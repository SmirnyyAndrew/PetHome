using FilesService.Core.ErrorManagment;
using FilesService.Core.ErrorManagment.Envelopes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace FilesService.Core.Validation;

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
            }
        }

        ResponseEnvelope envelope = ResponseEnvelope.Error(errors.ToArray());

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
